using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Ecommerce.Domain.DTO;
using Ecommerce.Domain.Entities;
using Ecommerce.Service.Infrastructure;

namespace Ecommerce.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administration")]
    public class ManageProductsController : ApiController
    {
          private readonly IRepositoryService<Product> _productService;

        public ManageProductsController( IRepositoryService<Product> productService)
        {
            _productService = productService;
        }

        // GET api/admin
        public IEnumerable<ProductDto> GetProducts()
        {
            return _productService.GetAll().Select(p => new ProductDto() { Id = p.Id, Name = p.Name, Price = p.Price , ActualCost = p.ActualCost });
        }

        // GET api/admin/5
        public Product GetProduct(int id)
        {
            var product = _productService.GetById(id);
            if (product == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return product;
        }


        // POST api/admin
        public HttpResponseMessage Post(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                var product = new Product()
                {
                    Name = productDto.Name,
                    Price = productDto.Price,
                    ActualCost = productDto.ActualCost,
                    CategoryId = 2
                };
                try
                {
                    _productService.Insert(product);
                    _productService.Save();
                }
                catch (Exception ex)
                {

                    return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                }

                var response = Request.CreateResponse(HttpStatusCode.Created, product);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = product.Id }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // PUT api/admin/5
       public HttpResponseMessage PutProduct(int id, ProductDto product)
        {
            if (ModelState.IsValid && id == product.Id)
            {
                var productToUpdate = _productService.GetById(id);
                if (productToUpdate == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                productToUpdate.Id = product.Id;
                productToUpdate.ActualCost = product.ActualCost;
                productToUpdate.Price = product.Price;
                productToUpdate.Name = product.Name; 
                try
                {
                    _productService.Update(productToUpdate);
                    _productService.Save();
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError , ex);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/admin/5
       public HttpResponseMessage Delete(int id)
        {
            var product = _productService.GetById(id);
            if (product == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            try
            {

                _productService.Delete(product.Id);
                _productService.Save();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError , ex);
            }
           
        }
    }
}
