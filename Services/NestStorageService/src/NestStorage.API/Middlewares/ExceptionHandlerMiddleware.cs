namespace NestStorage.API.Middlewares;

public static class ExceptionHandlerMiddleware
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    var statusCode = (int)HttpStatusCode.InternalServerError;
                    var message = contextFeature.Error.Message;

                    if (contextFeature.Error is IBaseException exception)
                    {
                        statusCode = exception.StatusCode;
                        message = exception.ErrorMessage;
                    }

                    context.Response.StatusCode = statusCode;
                    await context.Response.WriteAsJsonAsync(new ResponseDto { StatusCode = statusCode, Errors = new string[] { message } });
                    await context.Response.CompleteAsync();
                }
            });
        });

        return app;
    }
}