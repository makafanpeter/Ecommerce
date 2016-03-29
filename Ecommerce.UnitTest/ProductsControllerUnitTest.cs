using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Routing;
using Ecommerce.Data.Infrastructure;
using Ecommerce.Data.Repositories;
using Ecommerce.Domain.DTO;
using Ecommerce.Domain.Entities;
using Ecommerce.Service;
using Ecommerce.Service.Infrastructure;
using Ecommerce.Web.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Ecommerce.UnitTest
{
    [TestClass]
    public class ProductsControllerUnitTest
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IProductRepository> _productRepositoryMock;
        private IRepositoryService<Product> _productService;
        private IProductRepository _productRepository;


        [TestInitialize]
        public void Initialize()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _productService = new ProductService(_productRepositoryMock.Object, _unitOfWorkMock.Object);

            _productRepository = _productRepositoryMock.Object;

            var category1 = new Category()
            {
                Name = "Toys"

            };

            var category2 = new Category()
            {
                Name = "Food"

            };

            var category3 = new Category()
            {
                Name = "Tools"

            };
            var products = new List<Product>()            
            {
                new Product() {Id = 1, Name = "Tomato Soup", Price = 1.39M, ActualCost = .99M, Category =  category2},
                new Product() {Id = 2, Name = "Hammer", Price = 16.99M, ActualCost = 10, Category =  category3},
                new Product() {Id = 3, Name = "Yo yo", Price = 6.99M, ActualCost = 2.05M, Category =  category1},
                new Product() {Id = 4, Name = "Po Po", Price = 3.99M, ActualCost = 2.05M, Category =  category1 }

            };

            _productRepositoryMock.Setup(m => m.Add(It.IsAny<Product>())).Callback((Product product) =>
            {
                product.Id = products.Count + 1;
                products.Add(product);
            });


            _productRepositoryMock.Setup(m => m.GetAll()).Returns(products);

            _productRepositoryMock.Setup(m => m.Get(It.IsAny<Expression<Func<Product, bool>>>())).Returns((Expression<Func<Product, bool>> expression) =>
            {
                var data = products.Where(expression.Compile()).FirstOrDefault();
                return data;
            });

            _productRepositoryMock.Setup(m => m.Update(It.IsAny<Product>())).Callback(((Product product) =>
            {
                var t = products.Single(p => p.Id == product.Id);
                t.Id = product.Id;
                t.Name = product.Name;
                t.ActualCost = product.ActualCost;
                t.Price = product.Price;
                t.CategoryId = product.CategoryId;
            })).Verifiable();


            _productRepositoryMock.Setup(m => m.Delete(It.IsAny<Product>())).Callback(((Product p) =>
            {
                products.Remove(p);
            }));

            _productRepositoryMock.Setup(m => m.GetById(It.IsAny<long>())).Returns((long id) =>
            {
                var product = products.FirstOrDefault(i => i.Id == id);
                return product;
            });

            _unitOfWorkMock.Setup(i => i.Commit()).Callback(() =>
            {

            });

        }
        [TestMethod]
        public void TestMethod1()
        {
            var controller = new ProductsController(_productService)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var response = controller.Get();

            Assert.IsNotNull(response);
        }

         [TestMethod]
        public void TestMethod2()
        {

             //Arrange
            var controller = new ProductsController(_productService)
            {
                Request = new HttpRequestMessage()
                {
                    RequestUri = new Uri("http://localhost/api/products")
                },
                Configuration = new HttpConfiguration()
            };
            controller.Configuration.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/{controller}/{id}",
            defaults: new { id = RouteParameter.Optional }
            );

            controller.RequestContext.RouteData = new HttpRouteData(
            route: new HttpRoute(),
            values: new HttpRouteValueDictionary { { "controller", "products" } });

             //
            var response = controller.Get(1);

             //
            Assert.IsNotNull(response);
            //Assert.AreEqual("http://localhost/api/products/42", response.Headers.Location.AbsoluteUri);
        }

        [TestMethod]
        public  void TestMethod3()
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:1857"),
                DefaultRequestHeaders = { }

            };

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = client.GetAsync("api/products/1009").Result;
            
            Assert.IsTrue(response.IsSuccessStatusCode);
            var product = response.Content.ReadAsAsync<ProductDto>().Result;
            Assert.IsInstanceOfType(product, typeof (ProductDto));

        }
    }
}
