namespace TaxCalculator.Presentation.Api
{
    public class ExceptionHandleMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandleMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleException(ex, httpContext);
            }
        }

        private async Task HandleException(Exception ex, HttpContext httpContext)
        {
         //Customise Anyway needs...
            if (ex is ArgumentNullException)
            {
                httpContext.Response.StatusCode = 400;
                await httpContext.Response.WriteAsync($"error : {ex.Message}");
            }

            else
            {
                httpContext.Response.StatusCode = 500;

                await httpContext.Response.WriteAsync($"error : {ex.Message}");
            }

        }
    }
    public static class ExceptionHandleMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandleMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandleMiddleware>();
        }
    }
}
