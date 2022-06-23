using CompProgEdu.Configurations;
using CompProgEdu.Core.Data;
using CompProgEdu.Core.Features.Interfaces;
using CompProgEdu.Core.Features.Requests;
using CompProgEdu.Core.Features.Users;
using CompProgEdu.Features.Auth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CompProgEdu
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            SimpleInjectorConfiguration.ConfigureServices(services, configuration);

            FluentValidatorConfiguration.ConfigureServices(services);

            MvcConfiguration.ConfigureServices(services);

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();//TODO Add to AuthenticationConfiguration with JWT eventually with the rest of the configs

            DatabaseConfiguration.ConfigureDbServices(services, configuration);

            SwaggerConfiguration.ConfigureServices(services, configuration);

            JwtBearerConfiguration.ConfigureServices(services, configuration);
            services.AddControllersWithViews();
            services.AddHttpContextAccessor();
            services.AddTransient<ILoginRequest, LoginRequest>();
            //services.AddTransient<ISendGridRequest, SendToSendGridRequest>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
           //app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            CorsConfiguration.ConfigureApp(app);

            FluentValidatorConfiguration.ConfigureApp(app);

            SwaggerConfiguration.ConfigureApp(app, configuration);

            MvcConfiguration.ConfigureApp(app, env);

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "WebApp";

                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:3000");
                }
            });

            DatabaseConfiguration.ConfigureApp(app);
        }
    }
}
