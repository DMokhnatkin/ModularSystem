using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ModularSystem.Server.Common
{
    public class MappedExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public MappedExceptionFilterAttribute(Type exceptionType, HttpStatusCode httpStatusCode, bool onlyMessage = true)
        {
            ExceptionType = exceptionType;
            HttpStatusCode = httpStatusCode;
            OnlyMessage = onlyMessage;
        }

        public Type ExceptionType { get; }

        public HttpStatusCode HttpStatusCode { get; }

        public bool OnlyMessage { get; }

        /// <inheritdoc />
        public override void OnException(ExceptionContext context)
        {
            var exceptiontype = context.Exception.GetType();
            if (exceptiontype == ExceptionType)
            {
                var res = new ContentResult
                {
                    Content = OnlyMessage ? context.Exception.Message : context.Exception.ToString(),
                    StatusCode = (int)HttpStatusCode
                };
                context.Result = res;
                return;
            }
            base.OnException(context);
        }
    }
}
