using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NLog;

namespace Quantumart.QP8.WebMvc.Extensions.Logging;

internal class RequestLoggingMiddleware
{
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var sw = Stopwatch.StartNew();
        try
        {
            LogRequest(context);

            await _next(context);

            sw.Stop();
            LogResponseSuccessful(context, sw.ElapsedMilliseconds);
        }
        catch (Exception exception) when (LogResponseFaulted(context, sw.ElapsedMilliseconds, exception))
        {
        }
    }

    private void LogRequest(HttpContext context)
    {
        Logger.ForInfoEvent()
            .Message(LoggingConstants.RequestMessageTemplate, context.GetRequestMethod(), context.GetRequestUrl())
            .Property(LoggingConstants.EventIdProperty, EventIds.HttpRequestEventId)
            .LogCommonProperties(context)
            .Log();
    }

    private void LogResponseSuccessful(HttpContext context, long elapsed)
    {
        var statusCode = context.Response.StatusCode;

        Logger.ForInfoEvent()
            .Message(LoggingConstants.ResponseMessageTemplate, context.GetRequestMethod(), context.GetRequestUrl(), statusCode, elapsed)
            .Property(LoggingConstants.EventIdProperty, EventIds.HttpResponseEventId)
            .LogCommonProperties(context)
            .LogCommonResponseProperties(elapsed, statusCode)
            .Log();
    }

    private bool LogResponseFaulted(HttpContext context, long elapsed, Exception exception)
    {
        var statusCode = (int)HttpStatusCode.InternalServerError;

        Logger.ForInfoEvent()
            .Exception(exception)
            .Message(LoggingConstants.ResponseMessageTemplate, context.GetRequestMethod(), context.GetRequestUrl(), statusCode, elapsed)
            .Property(LoggingConstants.EventIdProperty, EventIds.HttpResponseEventId)
            .LogCommonProperties(context)
            .LogCommonResponseProperties(elapsed, statusCode)
            .Log();

        return false;
    }
}
