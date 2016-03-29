using System;
using System.Web.Mvc;

namespace Ecommerce.Web.Filters.Authorize
{
    public abstract class HttpUnauthorizedWithRedirectToResultBase : HttpUnauthorizedResult
    {
        protected ActionResult Result;

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            if (context.HttpContext.Request.IsAuthenticated)
            {
                context.HttpContext.Response.StatusCode = 200;
                InitializeResult(context);
                Result.ExecuteResult(context);
            }
            else
                base.ExecuteResult(context);
        }

        protected abstract void InitializeResult(ControllerContext context);
    }
}
