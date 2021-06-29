﻿using System.Collections.Generic;
using DocHelper.Domain.Common.Interfaces;
using DocHelper.Domain.Common.Services;
using DocHelper.Domain.Pipeline;
using DocHelper.Domain.Repository;
using DocHelper.Infrastructure.Cache.Configuration;
using DocHelper.Infrastructure.Cache.Policy;
using DocHelper.Infrastructure.Cache.Processor;
using DocHelper.Infrastructure.Persistence;
using DocHelper.Infrastructure.Persistence.Interceptors;
using DocHelper.Infrastructure.Pipeline.Builder;
using DocHelper.Infrastructure.Pipeline.Executor;
using DocHelper.Infrastructure.Pipeline.Extension;
using DocHelper.Infrastructure.Repository;
using DocHelper.Infrastructure.Services;
using DocHelper.Infrastructure.Swagger;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DocHelper.Infrastructure
{
    public static class Configuration
    {
        public static IServiceCollection ConfigureInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // swagger
            services.AddSwagger(configuration);
            
            // Cache section
            configuration.GetSection(nameof(CacheOptions)).Bind(new CacheOptions());
            services.Configure<CacheOptions>(configuration.GetSection(nameof(CacheOptions)));

            services.AddScoped<CacheInterceptor>();
            services.AddSingleton<ICacheProcessor, CacheProcessor>();
            services.AddSingleton<CachePolicyManager>();

            // DB section
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                        configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
                    .AddInterceptors(GetInterceptors(services))
            );

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            services.AddScoped<IApplicationDbTransaction, ApplicationDbTransaction>();

            services.AddSingleton<ILocationService, LocationService>();

            // Repo section
            services.AddTransient<ICityRepository, CityRepository>();
            services.AddTransient<ISpecialtyRepository, SpecialtyRepository>();
            services.AddTransient<IDoctorRepository, DoctorRepository>();

            services.AddScoped<PipelineBuilder>();
            services.AddScoped<IPipelineExecutor, PipelineExecutor>();
            
            services.ConfigurePipelines();

            return services;
        }

        private static IEnumerable<IInterceptor> GetInterceptors(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();

            IList<IInterceptor> interceptors = new List<IInterceptor>();

            interceptors.Add(serviceProvider.GetService<CacheInterceptor>());

            return interceptors;
        }
    }
}