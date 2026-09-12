using ECommerce.Application.Abstractions.Repositories;
using ECommerce.Domain.IRepositories;
using ECommerce.Infrastructure.Identity;
using ECommerce.Infrastructure.Persistence;
using ECommerce.Infrastructure.Persistence.Interceptors;
using ECommerce.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
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
            services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
            }).AddRoles<IdentityRole<int>>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                    .GetBytes(configuration["Jwt:Key"]!))
                };
            });

            services.AddAuthorization();

            services.AddScoped<IUnitOfWork>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IBrandReadRepository,BrandReadRepository>();
            services.AddScoped<ICategoryReadRepository,CategoryReadRepository>();
            services.AddScoped<IOrderRepository,OrderRepository>();
            services.AddScoped<IOrderReadRepository,OrderReadRepository>();
            services.AddScoped<IBasketRepository,BasketRepository>();
            services.AddScoped<IIdentityService,IdentityService>();
            services.AddScoped<ITokenService,TokenService>();
            services.AddHttpContextAccessor();

            services.AddScoped< ICurrentUserService,CurrentUserService>();
            services.AddScoped<IPaymentRepository,PaymentRepository>();
            services.AddScoped<IPaymentService,PaymentService>();

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            return services;

        }
    }
}
