using System.Collections.Generic;
using System.Data.Entity;
using Ecommerce.Data.Infrastructure;
using Ecommerce.Domain.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Ecommerce.Data.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
        public void AssignRole(string userName, List<string> roleNames)
        {
            var user = this.GetById(userName);
            user.Roles.Clear();
            foreach (string roleName in roleNames)
            {
                var role = this.DataContext.Roles.Find(roleName);
                var newRole = new IdentityUserRole() { RoleId = role.Id};
                
                user.Roles.Add(newRole);
            }

            this.DataContext.Users.Attach(user);
            this.DataContext.Entry(user).State = EntityState.Modified;
        }

    }

    public interface IUserRepository : IRepository<User>
    {
        void AssignRole(string userName, List<string> roleName);
    }
}