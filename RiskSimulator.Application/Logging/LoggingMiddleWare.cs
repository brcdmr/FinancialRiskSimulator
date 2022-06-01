using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IO;

namespace RiskSimulator.Application.Logging;

public class LoggingMiddleWare
{
    private readonly RequestDelegate _next;

    private static RecyclableMemoryStreamManager RecyclableMemory = new RecyclableMemoryStreamManager();

    public LoggingMiddleWare(RequestDelegate next)

    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ILogger<LoggingMiddleWare> _logger)
    {
        await _next(context);
        
        //context.Response.Headers.Add("accept","application/jso");
        //context.Response.Headers.Add("Content-Type","application/jso");
        return;
        

        var startTime = DateTime.Now;
        Stopwatch watch = Stopwatch.StartNew();

        var orgStream = context.Response.Body;

        using (var respBody = RecyclableMemory.GetStream())
        {
            context.Response.Body = respBody;
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString()); // formatla
            }
            
            watch.Stop();

            // time falan ekle
            // _logger.LogDebug(context.Request.Body.);

            await respBody.CopyToAsync(orgStream);
            context.Response.Body = orgStream;
        }
    }
}