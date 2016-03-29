using System.Web.Mvc;

namespace Ecommerce.Web.Filters.Authorize
{
    public class HttpUnauthorizedWithRedirectToViewResult : HttpUnauthorizedWithRedirectToResultBase
    {
        #region Ctors

        public HttpUnauthorizedWithRedirectToViewResult(string viewName, string area)
        {
            _viewName = string.IsNullOrWhiteSpace(viewName) ? viewName : viewName.Trim();
            _area = string.IsNullOrWhiteSpace(area) ? area : area.Trim();
        }

        #endregion

        #region Private Fields

        private readonly string _area;
        private readonly string _viewName;

        #endregion

        #region Overrides of HttpUnauthorizedWithRedirectToResultBase

        protected override void InitializeResult(ControllerContext context)
        {
            SetAreaRouteData(context);
            Result = new ViewResult
            {
                ViewName = _viewName,
            };
        }

        #endregion

        #region Methods

        private void SetAreaRouteData(ControllerContext context)
        {
            if (context.RequestContext.RouteData.DataTokens.ContainsKey("area"))
            {
                if (!string.IsNullOrWhiteSpace(_area))
                    context.RequestContext.RouteData.DataTokens["area"] = _area;
            }
            else
                context.RequestContext.RouteData.DataTokens.Add("area", _area);
        }

        #endregion
    }
}
