using Microsoft.AspNetCore.Http;
using NLog;
using Quantumart.QP8.BLL;

namespace Quantumart.QP8.WebMvc.Extensions.Logging;

internal static class LogEventBuilderExtensions
{
    public static LogEventBuilder LogCommonProperties(this LogEventBuilder builder, HttpContext context)
    {
        return builder.Property(LoggingConstants.RequestMethodProperty, context.GetRequestMethod())
            .Property(LoggingConstants.RequestUrlProperty, context.GetRequestUrl())
            .Property(LoggingConstants.CustomerCodeProperty, QPContext.CurrentCustomerCode);
    }

    public static LogEventBuilder LogCommonResponseProperties(this LogEventBuilder builder, long elapsed, int statusCode)
    {
        return builder.Property(LoggingConstants.SystemElapsedProperty, elapsed)
            .Property(LoggingConstants.StatusCodeProperty, statusCode);
    }
}
