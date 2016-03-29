using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Ecommerce.Domain.DTO;
using Ecommerce.Domain.Entities;
using Ecommerce.Service;
using Ecommerce.Web.Filters;
using Microsoft.AspNet.Identity;

namespace Ecommerce.Web.Controllers
{
    [IdentityBasicAuthentication]
    [Authorize]
    public class OrdersController : ApiController
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET api/Order
        public IEnumerable<OrderDetailDto> GetOrders()
        {
            var result = _orderService.GetOrders(User.Identity.Name);
            return result.Select(x => new OrderDetailDto { ConfirmationNumber = x.ConfirmationNumber, Id = x.Id }).ToList();
        }


        // GET api/Order/5
        public OrderDto GetOrder(int id)
        {
            var order = _orderService.GetOrder(id);
            if (order == null || order.User.UserName != User.Identity.Name)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return new OrderDto()
            {
                Details = order.OrderDetails.Select( d=> new ProductDetailDto()
                          {
                              ProductId = d.ProductId,
                              Product = d.Product.Name,
                              Price = d.Product.Price,
                              Quantity = d.Quantity
                          })
                          
            };
        }

        // PUT api/Order/5
        public HttpResponseMessage PutOrder(int id, Order order)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != order.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
               _orderService.UpdateOrder(order);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }


        // POST api/Order
        public HttpResponseMessage PostOrder(OrderDto dto)
        {

            if (dto != null && ModelState.IsValid)
            {
                var random = new Random();
                var orderNumber = random.Next(999999999);
                var order = new Order()
                {
                    
                    OrderDetails = dto.Details.Select( i=>  new OrderDetail() { ProductId = i.ProductId, Quantity = i.Quantity }).ToList(),
                    DateCreated = DateTime.Now,
                    ConfirmationNumber = orderNumber,
                    Amount = (double)dto.Details.Sum(item => item.Price * item.Quantity)
                };

               
                try
                {
                    _orderService.CreateOrder(order,User.Identity.GetUserName());
                }
                catch (Exception ex)
                {

                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
                }

                var response = Request.CreateResponse(HttpStatusCode.Created, order);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = order.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }



        // DELETE api/Order/5
        public HttpResponseMessage DeleteOrder(int id)
        {
            var order = _orderService.GetOrder(id);
            if (order == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

           

            try
            {
               _orderService.DeleteOrder(order);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, order);
        }

    }

    
}
