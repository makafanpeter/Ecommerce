using System;
using System.Web.Mvc;

namespace Ecommerce.Web.Filters.Authorize
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class AuthorizeWithRedirectToView : System.Web.Mvc.AuthorizeAttribute
    {
        #region Private Fields

        private const string DefaultActionOrViewName = "UnauthorizedAccess";
        private string _actionOrViewName;
        #endregion

        #region Properties

        /// <summary>
        ///   The name of the view to render on authorization failure. Default is "UnauthorizedAccess".
        /// </summary>
        public string ActionOrViewName
        {
            get
            {
                return string.IsNullOrWhiteSpace(_actionOrViewName)
                           ? DefaultActionOrViewName
                           : _actionOrViewName;
            }
            set { _actionOrViewName = value; }
        }

        public string Controller { get; set; }
        public string Area { get; set; }

        #endregion

        #region Overrides

        /// <summary>
        ///   Processes HTTP requests that fail authorization.
        /// </summary>
        /// <param name="filterContext"> Encapsulates the information for using <see cref="T:System.Web.Mvc.AuthorizeAttribute" /> . The <paramref
        ///    name="filterContext" /> object contains the controller, HTTP context, request context, action result, and route data. </param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.IsChildAction)
                base.HandleUnauthorizedRequest(filterContext);
            else
            {
                var factory = new HttpUnauthorizedWithRedirectToResultFactory();
                filterContext.Result = factory.GetInstance(Area, Controller, ActionOrViewName);
            }
        }

        #endregion
         
    }
}