using Microsoft.AspNetCore.Builder;

namespace Quantumart.QP8.WebMvc.Extensions.Logging;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app)
    {
        return app.UseMiddleware<RequestLoggingMiddleware>();
    }
}
