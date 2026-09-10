
using ECommerce.Application;
using ECommerce.Infrastructure;
using ECommerce.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace ECommerce
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplication();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider
                    .GetRequiredService<RoleManager<IdentityRole<int>>>();
                var userManager = scope.ServiceProvider
                    .GetRequiredService<UserManager<ApplicationUser>>();

                await IdentitySeeder.SeedRolesAsync(roleManager);
                await IdentitySeeder.SeedAdminAsync(userManager);
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
