namespace FClub.Backend.Common.HttpMessaging
{
    public interface IHttpClientService
    {
        Task<HttpResponseMessage> SendDeleteWithContent(string path, HttpContent content);
        Task SendResponse(string path, object? command, RequestType type, string token);
        Task SendResponse(string path, object? command, RequestType type);
    }
}
