using ECommerce.Application.Abstractions.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? UserId =>int.TryParse(_httpContextAccessor.HttpContext?.User
            .FindFirstValue(ClaimTypes.NameIdentifier),out var userId)? userId: null;

        public string? Email =>_httpContextAccessor.HttpContext?.User
            .FindFirstValue(ClaimTypes.Email);

        public string? FirstName =>_httpContextAccessor.HttpContext?.User
            .FindFirstValue(ClaimTypes.GivenName);

        public string? LastName => _httpContextAccessor.HttpContext?.User
            .FindFirstValue(ClaimTypes.Surname);
    }
}
