using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Users;

namespace CompProgEdu.Configurations
{
    public class DatabaseConfiguration
    {
        public static void ConfigureDbServices(IServiceCollection services, IConfiguration configuration)
        {
            //services.AddDbContext<DataContext>(options =>
            //    options.UseSqlServer(ConfigurationExtensions.GetConnectionString(configuration, "DataContext")));

            services.AddDbContext<DataContext>(options => { ConfigureCoreDbServices(options, configuration); });
        }

        public static void ConfigureCoreDbServices(DbContextOptionsBuilder databaseOptionsBuilder, IConfiguration configuration)
        {
            databaseOptionsBuilder
    .UseSqlServer(configuration.GetConnectionString(nameof(DataContext)), sqlOptionsBuilder =>
            {
                sqlOptionsBuilder.MigrationsAssembly(typeof(DataContext).Assembly.FullName);
                sqlOptionsBuilder.MigrationsHistoryTable("MigrationHistory", "system");
            });
        }

        public static void ConfigureApp(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetService<RoleManager<Role>>();

                var dataContext = scope.ServiceProvider.GetService<DataContext>();
                dataContext.Database.Migrate();
                dataContext.SeedData(userManager, roleManager);
            }
        }
    }
}
