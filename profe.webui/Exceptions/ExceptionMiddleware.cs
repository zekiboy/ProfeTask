using System;
using SendGrid.Helpers.Errors.Model;
using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace profe.webui.Exceptions
{
    public class ExceptionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }


        private static Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            int statusCode = GetStatusCode(exception);
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = statusCode;


            if (exception.GetType() == typeof(FluentValidation.ValidationException))
            {
                return httpContext.Response.WriteAsync(new ExceptionModel
                {
                    //ValidationException should be from FluentValidation. No Data Annotation
                    Errors = ((FluentValidation.ValidationException)exception).Errors.Select(x => x.ErrorMessage),
                    StatusCode = StatusCodes.Status400BadRequest
                }.ToString());

            }


            List<string> errors = new()
            {
                $"Hata Mesajı : {exception.Message}"
            };


            return httpContext.Response.WriteAsync(new ExceptionModel
            {
                Errors = errors,
                StatusCode = statusCode
            }.ToString());
        }


        private static int GetStatusCode(Exception exception) =>
            exception switch
            {
                BadRequestException => StatusCodes.Status400BadRequest,
                NotFoundException => StatusCodes.Status400BadRequest,
                FluentValidation.ValidationException => StatusCodes.Status422UnprocessableEntity,
                _ => StatusCodes.Status500InternalServerError
            };
    }

}

