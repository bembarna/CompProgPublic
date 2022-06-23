using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CompProgEdu.Core.Features.Users;
using System.Reflection;

namespace CompProgEdu.Core.Data
{
    public class DataContext : IdentityDbContext<User, Role, int, Features.Users.UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {

        public DataContext(
            DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(DataContext).GetTypeInfo().Assembly);
        }
    }
}
