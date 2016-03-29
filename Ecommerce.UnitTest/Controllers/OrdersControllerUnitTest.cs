using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using Ecommerce.Data.Infrastructure;
using Ecommerce.Data.Repositories;
using Ecommerce.Domain.DTO;
using Ecommerce.Domain.Entities;
using Ecommerce.Service;
using Ecommerce.Web.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Ecommerce.UnitTest.Controllers
{
    [TestClass]
    public class OrdersControllerUnitTest
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IOrderRepository> _orderRepositoryMock;
        private IOrderService _orderService;
        private IOrderRepository _orderRepository;
        private IUserRepository _userRepository;
        private Mock<HttpRequestContext> _controllerContextMock;
        private Mock<IPrincipal> _principalMock;
        private Mock<HttpRequestBase> _requestContextMock;
        private readonly string userName = "admin@ecommerce.ng";

        [TestInitialize]
        public void Initialize()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _controllerContextMock = new Mock<HttpRequestContext>();
            _principalMock = new Mock<IPrincipal>();
            _requestContextMock = new Mock<HttpRequestBase>();

            _principalMock.Setup(p => p.IsInRole("Administrator")).Returns(true);
            _principalMock.SetupGet(z => z.Identity.Name).Returns(userName);
            _principalMock.SetupGet(y => y.Identity.IsAuthenticated).Returns(true);


            _controllerContextMock.SetupGet(p => p.Principal).Returns(_principalMock.Object);

            
            
            _orderService = new OrderService(_userRepositoryMock.Object, _orderRepositoryMock.Object,
                _unitOfWorkMock.Object);

            _orderRepository = _orderRepositoryMock.Object;
            _userRepository = _userRepositoryMock.Object;

            var user = new User() { UserName = userName };
            var orders = new List<Order>()
            {
                new Order( ){ Id =1 ,User =  user, Amount =  300}
               
            };

            _orderRepositoryMock.Setup(m => m.GetAll()).Returns(orders);

            _orderRepositoryMock.Setup(o => o.Add(It.IsAny<Order>())).Callback((Order o) =>
            {
                o.Id = orders.Count + 1;
                orders.Add(o);
            });

            _userRepositoryMock.Setup(s => s.Get(It.IsAny<Expression<Func<User, bool>>>())).Returns(user);
           

            _unitOfWorkMock.Setup(i => i.Commit()).Callback(() =>
            {

            });

        }

        [TestMethod]
        public void TestMethod1()
        {
            var controller = new OrdersController(_orderService)
            {
                Request = new HttpRequestMessage()
                {
                    RequestUri = new Uri("http://localhost/api/orders")
                },
                Configuration = new HttpConfiguration()
            };

            controller.Configuration.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/{controller}/{id}",
            defaults: new { id = RouteParameter.Optional }
            );

            controller.RequestContext.Principal = _principalMock.Object;

            controller.RequestContext.RouteData = new HttpRouteData(
            route: new HttpRoute(),
            values: new HttpRouteValueDictionary { { "controller", "orders" } });

            var response = controller.GetOrders();

            Assert.IsNotNull(response);
        }



        [TestMethod]
        public void TestMethod2()
        {
            var controller = new OrdersController(_orderService)
            {
                Request = new HttpRequestMessage()
                {
                    RequestUri = new Uri("http://localhost/api/orders")
                },
                Configuration = new HttpConfiguration()
            };

            controller.Configuration.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/{controller}/{id}",
            defaults: new { id = RouteParameter.Optional }
            );

            controller.RequestContext.Principal = _principalMock.Object;

            controller.RequestContext.RouteData = new HttpRouteData(
            route: new HttpRoute(),
            values: new HttpRouteValueDictionary { { "controller", "orders" } });

            var dto = new OrderDto(){Details = new List<ProductDetailDto>()
            {
                new ProductDetailDto(){Product = "Sugar", Price = 200, ProductId = 1, Quantity = 1},
                new ProductDetailDto(){Product = "Bread", Price = 200, ProductId = 1, Quantity = 1}
            }};
            var response = controller.PostOrder(dto);

            var orders = _orderRepositoryMock.Object.GetAll();

            Assert.AreEqual(700, orders.Sum(x=>x.Amount));

            Assert.IsNotNull(response);

            Assert.AreEqual("http://localhost/api/orders/2", response.Headers.Location.AbsoluteUri);
        }

        [TestMethod]
        public void TestMethod3()
        {

            var client = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:1857"),
                DefaultRequestHeaders = { }

            };

            var dto = new OrderDto()
            {
                Details = new List<ProductDetailDto>()
            {
                new ProductDetailDto(){Product = "Sugar", Price = 200, ProductId = 1009, Quantity = 1},
                new ProductDetailDto(){Product = "Bread", Price = 200, ProductId = 1010, Quantity = 1}
            }
            };

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(
                    System.Text.Encoding.ASCII.GetBytes(
                        string.Format("{0}:{1}", "admin@ecommerce.ng", "Test123456"))));

            var response = client.PostAsJsonAsync("api/orders",dto).Result;
            
            Assert.IsTrue(response.IsSuccessStatusCode);
            
        }
    }
}
