using System;
using System.Web.Mvc;
using Ecommerce.Web.Filters.Authorize;

namespace Ecommerce.Web.Areas.Admin.Controllers
{
    [AuthorizeWithRedirectToView(Roles = "Administration", ActionOrViewName = "UnauthorizedAccess")]
    public class HomeController : Controller
    {
        //
        // GET: /Admin/Home/
        public ActionResult Index()
        {
            var apiUri = Url.HttpRouteUrl("AdminApi", new { controller = "ManageProducts" });
            ViewBag.ApiUrl = new Uri(Request.Url, apiUri).AbsoluteUri.ToString();
            return View();
        }
	}
}