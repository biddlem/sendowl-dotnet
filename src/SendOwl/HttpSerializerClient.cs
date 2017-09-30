using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SendOwl
{
    public interface IHttpSerializerClient : IDisposable
    {
        Task<T> GetAsync<T>(string relativeUrl);
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

        public async Task<T> GetAsync<T>(string url)
        {
            return LowercaseJsonSerializer.DeserializeObject<T>(await client.GetStringAsync(url));
        }

        public void Dispose()
        {
            client?.Dispose();
        }
    }
}
