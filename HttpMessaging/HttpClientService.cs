using FClub.Backend.Common.Exceptions;
using System.Text;
using System.Text.Json;

namespace FClub.Backend.Common.HttpMessaging
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClientServiceOptions _options;
        private readonly HttpClient _httpClient;
        private readonly string _hostName;
        private readonly string _serviceName;

        public HttpClientService(HttpClient httpClient, HttpClientServiceOptions options)
        {
            _options = options;
            _httpClient = httpClient;
            _hostName = options.HostName;
            _serviceName = options.ServiceName;
        }

        public async Task<HttpResponseMessage> SendDeleteWithContent(string path, HttpContent content)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, path)
            {
                Content = content
            };
            return await _httpClient.SendAsync(request);
        }

        public async Task SendResponse(string path, object command, RequestType type)
        {
            using var httpContent = new StringContent(
                JsonSerializer.Serialize(command),
                Encoding.UTF8,
                "application/json"
            );

            try
            {
                HttpResponseMessage response = type switch
                {
                    RequestType.Post => await _httpClient.PostAsync(_hostName + path, httpContent),
                    RequestType.Put => await _httpClient.PutAsync(_hostName + path, httpContent),
                    RequestType.Delete => await SendDeleteWithContent(_hostName + path, httpContent),
                    _ => throw new BadRequestException($"Request type {type} is not supported")
                };

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new BadRequestException($"{_serviceName ?? "Service"} error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new ServiceUnavailableException($"{_serviceName ?? "Service"} is unavailable: {ex.Message}");
            }
        }
    }
}
