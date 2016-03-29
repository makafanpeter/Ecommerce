using Ecommerce.Data.Infrastructure;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Data.Repositories
{
    public class ProductRepository: RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }


    public interface IProductRepository : IRepository<Product>
    {

    }
}