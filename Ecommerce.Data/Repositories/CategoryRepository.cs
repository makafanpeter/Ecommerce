using Ecommerce.Data.Infrastructure;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Data.Repositories
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }

    public interface ICategoryRepository : IRepository<Category>
    {

    }
}