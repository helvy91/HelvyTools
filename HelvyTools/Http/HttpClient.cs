using HelvyTools.Utils.Json;

using Net = System.Net.Http;

namespace HelvyTools.Http
{
    public class HttpClient
    {
        private readonly Net.HttpClient _client;

        public HttpClient(Net.HttpClient client)
        {
            _client = client;
        }

        public Task GetAsync(string url)
        {
            return _client.GetAsync(url);
        }

        public async Task<T> GetAsync<T>(string url)
        {
            var result = await _client.GetStringAsync(url);
            if (typeof(T) == typeof(string))
            {
                return (T)Convert.ChangeType(result, typeof(string));
            }

            return JsonUtils.Deserialize<T>(result, true);
        }

        public Task PutAsync(string url, HttpContent content)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Content = content;

            return SendRequest(request);
        }

        public async Task<T> PutAsync<T>(string url, HttpContent content)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Content = content;

            var response = await SendRequest(request);
            var contentString = await response.Content.ReadAsStringAsync();

            if (typeof(T) == typeof(string))
            {
                return (T)Convert.ChangeType(contentString, typeof(string));
            }

            return JsonUtils.Deserialize<T>(contentString, true);
        }

        public Task PostAsync(string url, HttpContent content)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = content;

            return SendRequest(request);
        }

        public Task<T> PostAsync<T>(string url, HttpContent content)
        {
            var headers = new Dictionary<string, string>();
            return PostAsync<T>(url, content, headers);
        }

        public async Task<T> PostAsync<T>(string url, HttpContent content, Dictionary<string, string> headers)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = content;
            
            foreach (var header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }

            var response = await SendRequest(request);
            var contentString = await response.Content.ReadAsStringAsync();

            if (typeof(T) == typeof(string))
            {
                return (T)Convert.ChangeType(contentString, typeof(string));
            }

            return JsonUtils.Deserialize<T>(contentString, true);
        }

        public Task DeleteAsync(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            return SendRequest(request);
        }

        public async Task<byte[]> DownloadFileContentAsync(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await SendRequest(request);

            using (var memoryStream = new MemoryStream())
            {
                await response.Content.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public async Task<HttpResponseMessage> SendRequest(HttpRequestMessage request)
        {
            var response = await _client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Request for {request.RequestUri} failed. Status code: {response.StatusCode}.");
            }

            return response;
        }    
    }
}
