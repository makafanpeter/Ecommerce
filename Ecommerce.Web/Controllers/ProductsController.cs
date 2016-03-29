using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Ecommerce.Domain.DTO;
using Ecommerce.Domain.Entities;
using Ecommerce.Service.Infrastructure;

namespace Ecommerce.Web.Controllers
{
    public class ProductsController : ApiController
    {
        private readonly IRepositoryService<Product> _productService;

        public ProductsController( IRepositoryService<Product> productService)
        {
            _productService = productService;
        }

        
        // GET api/products
        public IEnumerable<ProductDto> Get()
        {
            return _productService.GetAll().Select( p=> new  ProductDto() { Id = p.Id, Name = p.Name, Price = p.Price });
        }

        // GET api/products/5
        public ProductDto Get(int id)
        {
            var product =  _productService.GetById(id);
            if (product == null)
            {
                throw new HttpResponseException(
                        Request.CreateResponse(HttpStatusCode.NotFound));
               
            }
            return new ProductDto() { Id = product.Id, Name = product.Name, Price = product.Price };
        }

    }
}
