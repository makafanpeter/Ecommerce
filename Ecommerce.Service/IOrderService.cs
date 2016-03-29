using System.Collections.Generic;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Service
{
    public interface IOrderService
    {
        IEnumerable<Order> GetOrders(string username);

        void CreateOrder(Order order, string username);

        void DeleteOrder(Order order);
        void UpdateOrder(Order order);

        Order GetOrder(int id);

        void Save();
    }
}