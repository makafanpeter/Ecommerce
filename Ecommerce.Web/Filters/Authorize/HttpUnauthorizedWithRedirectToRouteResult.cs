using System.Web.Mvc;
using System.Web.Routing;

namespace Ecommerce.Web.Filters.Authorize
{
    public class HttpUnauthorizedWithRedirectToRouteResult : HttpUnauthorizedWithRedirectToResultBase
    {
        #region Ctors

        public HttpUnauthorizedWithRedirectToRouteResult(string action, string controller, string area)
        {
            _action = string.IsNullOrWhiteSpace(action) ? action : action.Trim();
            _controller = string.IsNullOrWhiteSpace(controller) ? controller : controller.Trim();
            _area = string.IsNullOrWhiteSpace(area) ? area : area.Trim();
        }

        #endregion

        #region Private Fields

        private readonly string _action;
        private readonly string _area;
        private readonly string _controller;

        #endregion

        #region Overrides of HttpUnauthorizedWithRedirectToResultBase

        protected override void InitializeResult(ControllerContext context)
        {
            Result = new RedirectToRouteResult(new RouteValueDictionary
                                                    {
                                                        {"area", _area},
                                                        {"controller", _controller},
                                                        {"action", _action}
                                                    });
        }

        #endregion
    }
}
