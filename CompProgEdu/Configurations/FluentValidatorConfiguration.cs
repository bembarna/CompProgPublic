using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using CompProgEdu.Core.Features.Behaviors;

namespace CompProgEdu.Configurations
{
    public static class FluentValidatorConfiguration
    {
        private static readonly Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.FullName.Contains("CompProgEdu.Core"));
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(BehaviorValidation<,>));
            services.AddValidatorsFromAssembly(assembly);

        }
        public static void ConfigureApp(IApplicationBuilder app)
        {

            app.UseExceptionHandler(x =>
            {
                x.Run(async context =>
                {
                    var errorFeat = context.Features.Get<IExceptionHandlerFeature>();
                    var exception = errorFeat.Error;

                    if (!(exception is ValidationException validationException))
                    {
                        throw exception;
                    }

                    var errors = validationException.Errors.Select(x => new
                    {
                        x.PropertyName,
                        x.ErrorMessage
                    });
                    var errorText = JsonSerializer.Serialize(errors);
                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(errorText, Encoding.UTF8);
                });
            });

        }
    }
}
