using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CompProgEdu.Configurations
{
    public static class MvcConfiguration
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(/*options=>
            {
                options.EnableEndpointRouting = false;
            }*/)
                .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<Startup>());
            services
                .AddRazorPages();

            services.Configure<RouteOptions>(options => { options.LowercaseUrls = true; });
        }

        public static void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors();
            app.UseHttpsRedirection();
            JwtBearerConfiguration.ConfigureApp(app);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}
