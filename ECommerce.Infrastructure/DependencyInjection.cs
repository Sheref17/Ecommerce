using ECommerce.Application.Abstractions.Repositories;
using ECommerce.Domain.IRepositories;
using ECommerce.Infrastructure.Persistence;
using ECommerce.Infrastructure.Persistence.Interceptors;
using ECommerce.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services ,
            IConfiguration configuration) 
        {
            services.AddScoped<DomainEventDispatcherInterceptor>();
            services.AddDbContext<ApplicationDbContext>((serviceProvider , options) =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

                options.AddInterceptors(serviceProvider.GetRequiredService<DomainEventDispatcherInterceptor>());
            });

            services.AddScoped<IUnitOfWork>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IBrandReadRepository,BrandReadRepository>();
            services.AddScoped<ICategoryReadRepository,CategoryReadRepository>();
            services.AddScoped<IOrderRepository,OrderRepository>();
            services.AddScoped<IOrderReadRepository,OrderReadRepository>();

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            return services;

        }
    }
}
