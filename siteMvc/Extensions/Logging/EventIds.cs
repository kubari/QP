using Microsoft.Extensions.Logging;

namespace Quantumart.QP8.WebMvc.Extensions.Logging;

public static class EventIds
{
    public const int HttpRequest = 100;
    public const int HttpResponse = 101;

    public static readonly EventId HttpRequestEventId = new EventId(HttpRequest, nameof(HttpRequest));
    public static readonly EventId HttpResponseEventId = new EventId(HttpResponse, nameof(HttpResponse));
}
