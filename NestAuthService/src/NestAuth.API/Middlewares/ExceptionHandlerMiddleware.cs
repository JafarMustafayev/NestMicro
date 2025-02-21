namespace NestAuth.API.Middlewares;

public static class ExceptionHandlerMiddleware
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                int statusCode = (int)HttpStatusCode.InternalServerError;
                string message = contextFeature.Error.Message;

                //string message = "Internal Server Error";

                if (contextFeature != null && contextFeature.Error is IBaseException)
                {
                    var exception = (IBaseException)contextFeature.Error;
                    statusCode = exception.StatusCode;
                    message = exception.ErrorMessage;
                }
                context.Response.StatusCode = statusCode;
                await context.Response.WriteAsJsonAsync(new ResponseDto { StatusCode = statusCode, Message = message });
                await context.Response.CompleteAsync();
            });
        });

        return app;
    }
}