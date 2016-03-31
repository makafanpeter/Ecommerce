using System.Collections.Generic;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enum;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Ecommerce.Data.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EcommerceContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(EcommerceContext context)
        {
            var category1 = new Category()
            {
                Name = "Toys"
               
            };

            var category2 = new Category()
            {
                Name = "Food"

            };

            var category3 = new Category()
            {
                Name = "Tools"

            };

            context.Categories.Add(category1);

            context.Categories.Add(category2);

            context.Categories.Add(category3);

            var products = new List<Product>()            
            {
                new Product() { Name = "Tomato Soup", Price = 1.39M, ActualCost = .99M, Category =  category2},
                new Product() { Name = "Hammer", Price = 16.99M, ActualCost = 10, Category =  category3},
                new Product() { Name = "Yo yo", Price = 6.99M, ActualCost = 2.05M, Category =  category1},
                new Product() { Name = "Po Po", Price = 3.99M, ActualCost = 2.05M, Category =  category1 }

            };

            products.ForEach(p => context.Products.Add(p));
            context.SaveChanges();

            var roles  = new[]{UserTypeEnum.Administrator.ToString(), UserTypeEnum.StandardUser.ToString()};

            foreach (var role in roles)
            {
                if (context.Roles.Any(x => x.Name == role)) continue;
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var newRole = new IdentityRole(){Name =  role};
                manager.Create(newRole);
            }
        }
    }
}
