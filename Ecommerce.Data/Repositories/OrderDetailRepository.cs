using Ecommerce.Data.Infrastructure;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Data.Repositories
{
    public class OrderDetailRepository: RepositoryBase<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }

    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {

    }
}