using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Ecommerce.Domain.Entities;
using Ecommerce.Service.Infrastructure;

namespace Ecommerce.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IRepositoryService<Product> _productService;
        public HomeController(IRepositoryService<Product> productService)
        {
            _productService = productService;
        }
        public ActionResult Index()
        {
            var products = _productService.GetAll();
            return View(products);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}