using System;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace Quantumart.QP8.WebMvc.Extensions.Logging;

public static class HttpContextExtensions
{
    private const string Unknown = "unknown";
    private const string PortSeparator = ":";
    private const char ColonChar = ':';
    private const char SpaceChar = ' ';

    public static string GetUserAgent(this HttpContext context) =>
        context.Request.Headers.UserAgent.FirstOrDefault();

    public static string GetRequestUrl(this HttpContext context) =>
        context.Request.GetDisplayUrl();

    public static string GetRequestPathBase(this HttpContext context) =>
        context.Request.PathBase.Value;

    public static string GetRequestMethod(this HttpContext context) =>
        context.Request.Method;

    public static string GetRequestCookies(this HttpContext context)
    {
        var builder = new StringBuilder();
        foreach (var cookie in context.Request.Cookies)
        {
            builder
                .Append(cookie.Key)
                .Append(ColonChar)
                .Append(SpaceChar)
                .Append(cookie.Value)
                .AppendLine();
        }

        return builder.ToString();
    }

    public static string GetClientIp(this HttpContext context)
    {
        var proxy = context.Request.Headers["x-forwarded-for"].FirstOrDefault();
        if (string.IsNullOrEmpty(proxy))
        {
            return context.Connection?.RemoteIpAddress?.ToString() ?? Unknown;
        }

        var addresses = proxy.Split(',');
        if (addresses.Any())
        {
            var address = addresses[0];
            // If IP contains port, it will be after the last : (IPv6 uses : as delimiter and could have more of them)
            return address.Contains(PortSeparator)
                ? address.Substring(0, address.LastIndexOf(PortSeparator, StringComparison.Ordinal))
                : address;
        }

        return Unknown;
    }
}
