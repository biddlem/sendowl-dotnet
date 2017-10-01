using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SendOwl
{
    public interface IHttpSerializerClient : IDisposable
    {
        Task<T> GetAsync<T>(string relativeUrl);
        Task DeleteAsync(string relativeUrl);
        Task<TResult> PostMultipartAsync<TResult, YObject>(string relativeUrl, YObject content, string resource);
    }

    public class HttpSerializerClient : IHttpSerializerClient
    {
        private readonly HttpClient client;

        public HttpSerializerClient(string baseUrl, string apiKey, string apiSecret)
        {
            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
            };
            client = new HttpClient(handler) { BaseAddress = new Uri(baseUrl) };
            var payload = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{apiKey}:{apiSecret}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", payload);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
        }

        public async Task<T> GetAsync<T>(string relativeUrl)
        {
            return LowercaseJsonSerializer.DeserializeObject<T>(await client.GetStringAsync(relativeUrl));
        }

        public async Task DeleteAsync(string relativeUrl)
        {
            (await client.DeleteAsync(relativeUrl)).EnsureSuccessStatusCode();
        }

        public async Task<TResult> PostMultipartAsync<TResult, YObject>(string relativeUrl, YObject obj, string resource)
        {
            var form = new MultipartFormDataContent();
            foreach(var prop in typeof(YObject).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                var name = prop.Name.ToLowerInvariant();
                var value = prop.GetValue(obj, null);
                var defaultValue = GetDefault(prop.PropertyType);
                if (value == null || (name == "id" && value.Equals(defaultValue))) continue;
                form.Add(new StringContent(value.ToString()), $"{resource}[{name}]");
            }
            var response = await client.PostAsync(relativeUrl, form);
            response.EnsureSuccessStatusCode();
            return LowercaseJsonSerializer.DeserializeObject<TResult>(await response.Content.ReadAsStringAsync());
        }

        private static object GetDefault(Type type)
        {
            if (type.GetTypeInfo().IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }

        public void Dispose()
        {
            client?.Dispose();
        }
    }
}
