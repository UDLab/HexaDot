using Microsoft.AspNetCore.Cors.Infrastructure;

namespace HexaDot.Features.Security
{
    public static class HexaDotCorsPolicyFactory
    {
        public static CorsPolicy BuildHexaDotOpenCorsPolicy()
        {
            var corsBuilder = new CorsPolicyBuilder();

            corsBuilder.AllowAnyHeader();
            corsBuilder.AllowAnyMethod();
            // we need to revisit this and build a whitelise of IP addresses
            corsBuilder.AllowAnyOrigin();
            corsBuilder.AllowCredentials();

            return corsBuilder.Build();
        }
    }
}
