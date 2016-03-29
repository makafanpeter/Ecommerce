    using System.Collections.Generic;
using System.Linq;
using Ecommerce.Data.Infrastructure;
using Ecommerce.Data.Repositories;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Service
{
    public class OrderService: IOrderService
    {

        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderRepository _orderRepository;
        public OrderService(IUserRepository userRepository, IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Order> GetOrders(string username)
        {
          var user =  _userRepository.Get(u=>u.UserName == username);
          return user !=null ? _orderRepository.GetAll().Where(u => u.User == user) : null;
        }

        public void CreateOrder(Order order, string username)
        {
            var user = _userRepository.Get(p=>p.UserName == username);
            order.User = user;
            order.UserId = user.Id;
            _orderRepository.Add(order);
            Save();
        }

        public void DeleteOrder(Order order)
        {
            _orderRepository.Delete(order);
            Save();
        }

        public void UpdateOrder(Order order)
        {
            _orderRepository.Update(order);
            Save();
        }

        public Order GetOrder(int id)
        {
            var order = _orderRepository.Get(x => x.Id == id, includeProperties: "User,OrderDetails,OrderDetails.Product").FirstOrDefault();
            return order;
        }


        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}