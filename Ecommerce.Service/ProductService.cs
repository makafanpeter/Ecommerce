using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ecommerce.Data.Infrastructure;
using Ecommerce.Data.Repositories;
using Ecommerce.Domain.Entities;
using Ecommerce.Service.Infrastructure;
namespace Ecommerce.Service
{
    public class ProductService : RepositoryService<Product>, IRepositoryServiceGet<Product>
    {
          public  ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork)
            : base(productRepository, unitOfWork)
        {

        }

        public IEnumerable<Product> Get(Expression<Func<Product, bool>> filter = null,
             Func<IQueryable<Product>, IOrderedQueryable<Product>> orderBy = null,
             string includeProperties = "")
        {
            var products = GenericRepository.Get(filter, orderBy, includeProperties);
            return products;
        }
    }
}