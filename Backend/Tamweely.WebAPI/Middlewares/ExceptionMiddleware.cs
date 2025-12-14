using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Tamweely.WebAPI.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    public ExceptionMiddleware(RequestDelegate next) { _next = next; }
    public async Task Invoke(HttpContext ctx)
    {
        try
        {
            await _next(ctx);
        }
        catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
        {
            ctx.Response.ContentType = "application/problem+json";
            ctx.Response.StatusCode = StatusCodes.Status400BadRequest;

            var problem = new
            {
                type = "https://httpstatuses.com/400",
                title = "Validation error",
                status = 400,
                errors = new Dictionary<string, string[]>
                {
                    ["email"] = new[] { "Email already exists." }
                }
            };

            await ctx.Response.WriteAsync(JsonSerializer.Serialize(problem));
        }
        catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
        {
            ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
            ctx.Response.ContentType = "application/problem+json";
            var problem = new
            {
                type = "https://httpstatuses.com/400",
                title = "Validation error",
                status = 400,
                errors = new Dictionary<string, string[]>
                {
                    ["email"] = new[] { "Email already exists." }
                }
            };
            await ctx.Response.WriteAsync(JsonSerializer.Serialize(problem));
        }
    }

    private static bool IsUniqueConstraintViolation(DbUpdateException ex)
    {
        if (ex.InnerException is SqlException sqlEx)
        {
            return sqlEx.Number == 2601 || sqlEx.Number == 2627;
        }

        return false;
    }
}