using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CompProgEdu.Configurations
{
    public static class CorsConfiguration
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
        }

        public static void ConfigureApp(IApplicationBuilder app)
        {
            var origins = "http://localhost:3000";
            app.UseCors(builder =>
            {
                builder
                    .WithOrigins(origins)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        }
    }
}
