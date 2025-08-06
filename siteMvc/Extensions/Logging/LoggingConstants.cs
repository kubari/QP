namespace Quantumart.QP8.WebMvc.Extensions.Logging;

public static class LoggingConstants
{
    public const string RequestMessageTemplate = "HTTP Request {requestMethod} {requestUrl}";
    public const string ResponseMessageTemplate = "HTTP Response {requestMethod} {requestUrl} returned {statusCode} in {systemElapsed}ms";

    public const string RequestMethodProperty = "requestMethod";
    public const string RequestUrlProperty = "requestUrl";
    public const string CustomerCodeProperty = "customerCode";
    public const string SystemElapsedProperty = "systemElapsed";
    public const string StatusCodeProperty = "statusCode";
    public const string EventIdProperty = "eventId";
}
