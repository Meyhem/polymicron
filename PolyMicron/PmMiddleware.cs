using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;

namespace ProjectPlaguemangler
{
    public static class PmMiddleware
    {
        public static IApplicationBuilder UsePmNotFoundHandler(this IApplicationBuilder self)
        {
            self.Use(async (ctx, next) =>
            {
                await next();

                if (ctx.Response.StatusCode == 404 && !ctx.Response.HasStarted)
                {
                    //Re-execute the request so the user gets the error page
                    string originalPath = ctx.Request.Path.Value;
                    ctx.Items["originalPath"] = originalPath;
                    ctx.Request.Path = "/not-found";
                    await next();
                }
            });

            return self;
        }

        public static IApplicationBuilder UsePmVisitorId(this IApplicationBuilder self)
        {
            self.Use(async (ctx, next) =>
            {
                if (!ctx.Request.Cookies.ContainsKey("uvi"))
                {
                    ctx.Response.Cookies.Append("uvi", Guid.NewGuid().ToString(), new CookieOptions
                    {
                        HttpOnly = true,
                        Expires = DateTime.Now.AddYears(1)
                    });
                }

                await next();
            });

            return self;
        }
    }
}
