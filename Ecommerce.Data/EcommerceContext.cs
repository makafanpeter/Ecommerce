using System;
using System.Collections.Generic;
using System.Data.Entity;
using Ecommerce.Domain;
using System.Data.Entity.ModelConfiguration.Conventions;
using Ecommerce.Domain.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Ecommerce.Data
{
    public class EcommerceContext : IdentityDbContext<User>
    {
        public EcommerceContext() : base("name=DefaultConnection")
        {
            
        }

        public EcommerceContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
      
        public DbSet<Category> Categories { get; set; }


        public virtual void Commit()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        //    modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        //    modelBuilder.Entity<Instructor>()
        //        .HasOptional(p => p.OfficeAssignment).WithRequired(p => p.Instructor);
        //    modelBuilder.Entity<Orders>()
        //        .HasMany(c => c.UserOrders).WithMany(i => i.User)
        //        .Map(t => t.MapLeftKey("CourseID")
        //            .MapRightKey("PersonID")
        //            .ToTable("CourseInstructor"));
        //    modelBuilder.Entity<Department>()
        //        .HasOptional(x => x.Administrator);
        }


    }
}