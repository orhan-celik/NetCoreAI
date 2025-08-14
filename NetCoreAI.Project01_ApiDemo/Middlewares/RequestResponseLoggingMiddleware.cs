using Serilog;
using System.Text;

namespace NetCoreAI.Project01_ApiDemo.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Request.EnableBuffering();
            var requestBody = await ReadStreamAsync(context.Request.Body);

            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                await _next(context);
            }
            finally
            {
                // Response
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                // Serilog ile logla, MSSQL sink otomatik tabloya yazacak
                Log.ForContext("RequestBody", string.IsNullOrEmpty(requestBody) ? "(empty)" : requestBody)
                   .ForContext("ResponseBody", string.IsNullOrEmpty(responseText) ? "(empty)" : responseText)
                   .Information("HTTP {Method} {Path} responded {StatusCode}",
                        context.Request.Method,
                        context.Request.Path,
                        context.Response.StatusCode);

                // Response'u geri yaz
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private async Task<string> ReadStreamAsync(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(stream, Encoding.UTF8, true, 1024, true);
            var text = await reader.ReadToEndAsync();
            stream.Seek(0, SeekOrigin.Begin);
            return text;
        }
    }
}
