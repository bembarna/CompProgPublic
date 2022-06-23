using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using System;
using System.Linq;
using System.Reflection;

namespace CompProgEdu.Configurations
{//THIS FILE IS NON WORKING AND PROB DOESNT DO MUCH TBH.... MAYBE WE WILL FIX LATER, MAYBE WE WILL SCRAP? 
    public static class SimpleInjectorConfiguration
    {
        private static readonly Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.FullName.Contains("CompProgEdu.Core"));

        private static readonly Container container = new Container();

        public static void ConfigureServices(IServiceCollection services,
            IConfiguration configuration)
        {
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            InjectAutoMapper(services);
            InjectMediator(services);
            container.Verify();
        }

        public static void InjectMediator(IServiceCollection services)
        {
            services.AddMediatR(typeof(Startup).Assembly, assembly);
        }

        public static void InjectAutoMapper(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup).Assembly, assembly);

            var profiles = assembly.GetTypes().Where(x => typeof(Profile).IsAssignableFrom(x));

            var mapconfig = new MapperConfiguration(cfg =>
            {
                foreach (var profile in profiles)
                {
                    cfg.AddProfile(profile);
                }
            });

            container.RegisterInstance<IMapper>(mapconfig.CreateMapper());
        }

    }
}
