using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SendOwl
{
    public interface IHttpSerializerClient : IDisposable
    {
        Task<T> GetAsync<T>(string relativeUrl);
        Task<T> PostAsync<T>(string relativeUrl, T obj);
        Task PostNoResponseAsync(string relativeUrl, object obj = null);
        Task PutAsync<T>(string relativeUrl, T obj);
        Task DeleteAsync(string relativeUrl);
        Task<TResult> PostMultipartAsync<TResult, YObject>(string relativeUrl, YObject obj, string resource);
        Task<TResult> PostMultipartAsync<TResult, YObject>(string relativeUrl, YObject obj, string resource, Stream stream, string fileName);
    }

    public class HttpSerializerClient : IHttpSerializerClient
    {
        private readonly HttpClient client;
        private readonly SemaphoreSlim httpSemaphore;
        private const string JsonContentType = "application/json";

        public HttpSerializerClient(string baseUrl, string apiKey, string apiSecret, int maxConcurrentRequests = 1)
        {
            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
            };
            client = new HttpClient(handler) { BaseAddress = new Uri(baseUrl) };
            var payload = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{apiKey}:{apiSecret}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", payload);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(JsonContentType));
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            httpSemaphore = new SemaphoreSlim(maxConcurrentRequests);
        }

        public async Task<T> GetAsync<T>(string relativeUrl)
        {
            return await LimitConcurrentRequests(async () =>
                 LowercaseJsonSerializer.DeserializeObject<T>(await client.GetStringAsync(relativeUrl)));
        }

        public async Task PostNoResponseAsync(string relativeUrl, object obj = null)
        {
            {
                StringContent httpContent = null;
                if(obj != null)
                {
                    var json = LowercaseJsonSerializer.SerializeObject(obj);
                    httpContent = new StringContent(json, Encoding.UTF8, JsonContentType);
                }
                
                var response = await LimitConcurrentRequests(async () => await
                     client.PostAsync(relativeUrl, httpContent));
                var content = await response.Content.ReadAsStringAsync();

                try
                {
                    response.EnsureSuccessStatusCode();
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(content, ex);
                }
            }
        }

        public async Task<T> PostAsync<T>(string relativeUrl, T obj)
        {
            var json = LowercaseJsonSerializer.SerializeObject(obj);
            var response = await LimitConcurrentRequests(async () => await
                 client.PostAsync(relativeUrl, new StringContent(json, Encoding.UTF8, JsonContentType)));
            var content = await response.Content.ReadAsStringAsync();

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(content, ex);
            }
            
            return LowercaseJsonSerializer.DeserializeObject<T>(content);
        }

        public async Task PutAsync<T>(string relativeUrl, T obj)
        {
            var json = LowercaseJsonSerializer.SerializeObject(obj);
            var response = await LimitConcurrentRequests(async () => await
                client.PutAsync(relativeUrl, new StringContent(json, Encoding.UTF8, JsonContentType)));
            var content = await response.Content.ReadAsStringAsync();

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(content, ex);
            }
        }

        public async Task DeleteAsync(string relativeUrl)
        {
            (await LimitConcurrentRequests(async () => await client.DeleteAsync(relativeUrl))).EnsureSuccessStatusCode();
        }

        public async Task<TResult> PostMultipartAsync<TResult, YObject>(string relativeUrl, YObject obj, string resource)
        {
            return await LimitConcurrentRequests(async () => await 
                PostMultipartAsync<TResult, YObject>(relativeUrl, obj, resource, null, null));
        }

        public async Task<TResult> PostMultipartAsync<TResult, YObject>(string relativeUrl, YObject obj, string resource, Stream stream, string fileName)
        {
            var form = new MultipartFormDataContent();
            foreach (var prop in typeof(YObject).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                if (prop.GetSetMethod() == null || !prop.CanWrite || !prop.CanRead) continue; //ignore not settable properties;
                var name = prop.Name.ToLowerInvariant();
                var value = prop.GetValue(obj, null);
                var defaultValue = GetDefault(prop.PropertyType);
                if (value == null || (name == "id" && value.Equals(defaultValue))) continue; //ignore id property if it is null or default
                form.Add(new StringContent(value.ToString()), $"{resource}[{name}]");
            }

            if(stream != null)
            {
                var fileContent = new StreamContent(stream);
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
                form.Add(fileContent, $"{resource}[attachment]", fileName);
            }
            var response = await LimitConcurrentRequests(async () => await client.PostAsync(relativeUrl, form));
            var content = await response.Content.ReadAsStringAsync();
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(content, ex);
            }
            return LowercaseJsonSerializer.DeserializeObject<TResult>(content);
        }

        private static object GetDefault(Type type)
        {
            if (type.GetTypeInfo().IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }

        private async Task<T> LimitConcurrentRequests<T>(Func<Task<T>> action)
        {
            try
            {
                await httpSemaphore.WaitAsync();
                return await action().ConfigureAwait(false);
            }
            finally
            {
                httpSemaphore.Release();
            }
        }

        public void Dispose()
        {
            client?.Dispose();
        }
    }
}
