using Microsoft.AspNetCore.Http;

namespace CastAmNow.Api.Infrastructure.Extensions
{
    public static class UtilityExtension
    {
        public static string? GetClientIp(this HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress?.ToString();

            // Handle proxy headers (if behind load balancer or reverse proxy)
            if (context.Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            }

            return ip;
        }

        public static async Task<string> GetLocationFromIpAsync(this string? ip)
        {
            if (string.IsNullOrWhiteSpace(ip) || ip == "127.0.0.1" || ip == "::1")
            {
                return string.Empty;
            }
            using var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync($"https://ipapi.co/{ip}/city/");
            return response; // or deserialize to a model
        }
    }
}
