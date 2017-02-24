using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ModularSystem.Server.Common
{
    public class MappedExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public MappedExceptionFilterAttribute(Type exceptionType, HttpStatusCode httpStatusCode)
        {
            ExceptionType = exceptionType;
            HttpStatusCode = httpStatusCode;
        }

        public Type ExceptionType { get; }

        public HttpStatusCode HttpStatusCode { get; }

        /// <inheritdoc />
        public override void OnException(ExceptionContext context)
        {
            var exceptiontype = context.Exception.GetType();
            if (exceptiontype == ExceptionType)
            {
                var res = new ContentResult
                {
                    Content = context.Exception.ToString(),
                    StatusCode = (int)HttpStatusCode
                };
                context.Result = res;
                return;
            }
            base.OnException(context);
        }
    }
}
