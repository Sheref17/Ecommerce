using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;
    }
}
