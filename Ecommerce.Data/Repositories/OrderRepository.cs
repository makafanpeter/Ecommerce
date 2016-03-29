using Ecommerce.Data.Infrastructure;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Data.Repositories
{
    public class OrderRepository: RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }

    public interface IOrderRepository : IRepository<Order>
    {

    }
}