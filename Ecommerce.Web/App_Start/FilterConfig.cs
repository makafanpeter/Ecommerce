using System.Web;
using System.Web.Mvc;
using Ecommerce.Web.Filters;
using Ecommerce.Web.Filters.Authorize;

namespace Ecommerce.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthorizeWithRedirectToView());
           
        }
    }
}
