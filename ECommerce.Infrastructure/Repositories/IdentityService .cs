using ECommerce.Application.Abstractions.Repositories;
using ECommerce.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<(bool Succeeded,int UserId,IEnumerable<string> Errors)>
            CreateUserAsync( string firstName,string lastName,string email,
            string password,CancellationToken cancellationToken)
        {
            var user = new ApplicationUser
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                UserName = email
            };

            var result = await _userManager.CreateAsync(user,password);

            if (!result.Succeeded)
            {
                return (false,0,result.Errors.Select(x => x.Description));
            }

            return (true,user.Id,[]);
        }

        public async Task<(bool Succeeded,int UserId,IEnumerable<string> Errors)>ValidateCredentialsAsync(
            string email,string password,CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                return (false,0,["Invalid email or password."]);
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user,password);

            if (!passwordValid)
            {
                return (false,0,["Invalid email or password."]);
            }

            return (true,user.Id,[]);
        }

        public async Task<bool> AddToRoleAsync(int userId,string role,
            CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
                return false;

            var result = await _userManager.AddToRoleAsync(user,role);

            return result.Succeeded;
        }
    }
}
