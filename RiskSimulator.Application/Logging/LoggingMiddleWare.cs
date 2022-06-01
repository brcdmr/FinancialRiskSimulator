using System.Diagnostics;
using System.Text;
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
        // await _next(context);
        // return;
        //

        var startTime = DateTime.Now;
        Stopwatch watch = Stopwatch.StartNew();

        var request = await GetRequestBody(context.Request, _logger);
        var orgStream = context.Response.Body;

        _logger.LogDebug($"Endpoint: {context.Request.Path} - RequestBody: {request} - startTime: {startTime}");

        using (var respBody = RecyclableMemory.GetStream())
        {
            context.Response.Body = respBody;
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            
            watch.Stop();

            var response = ReadResponseBody(context, _logger);
       
             _logger.LogDebug($"Endpoint: {context.Request.Path} - ResponseBody: {response} - DurationMS: {watch.Elapsed.TotalMilliseconds}");

            await respBody.CopyToAsync(orgStream);
            context.Response.Body = orgStream;
        }
    }
    

    private static async Task<string> GetRequestBody(HttpRequest tmpRequest, ILogger<LoggingMiddleWare> _logger)
    {
        tmpRequest.EnableBuffering();

        string reqBody = "";

        using (var reader = new StreamReader(tmpRequest.Body, encoding: Encoding.UTF8,
                   detectEncodingFromByteOrderMarks: true, bufferSize: 1024, leaveOpen: true))

        {
            reqBody = await reader.ReadToEndAsync();
        }


        tmpRequest.Body.Seek(0, SeekOrigin.Begin);

        return reqBody;
    }


    public static string ReadResponseBody(HttpContext context, ILogger<LoggingMiddleWare> _logger)
    {
        try
        {
            var responseBody = context.Response.Body;
            if (!responseBody.CanSeek)
                return "";

            responseBody.Seek(0, SeekOrigin.Begin);
            using (var tmpStream = new MemoryStream())

            {
                responseBody.CopyTo(tmpStream);

                responseBody.Seek(0, SeekOrigin.Begin);

                return Encoding.UTF8.GetString(tmpStream.ToArray());
            }
        }

        catch (Exception ex)

        {
            _logger.LogError($"Error while reading response body.", ex);
        }


        return "[Error while reading body]";
    }

}