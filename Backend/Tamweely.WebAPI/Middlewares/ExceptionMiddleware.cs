using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Tamweely.WebAPI.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    public ExceptionMiddleware(RequestDelegate next) { _next = next; }
    public async Task Invoke(HttpContext ctx)
    {
        try { await _next(ctx); }
        catch (Exception ex)
        {
            ctx.Response.ContentType = "application/problem+json";
            ctx.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var problem = new { type = "about:blank", title = "An error occurred", status = ctx.Response.StatusCode, detail = ex.Message };
            await ctx.Response.WriteAsync(JsonSerializer.Serialize(problem));
        }
    }
}