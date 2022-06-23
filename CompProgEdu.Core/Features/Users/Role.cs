using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompProgEdu.Core.Features.Users
{
    public class Role : IdentityRole<int>
    {
        public List<UserRole> Users { get; set; } = new List<UserRole>();

        public List<RoleClaim> RoleClaims { get; set; } = new List<RoleClaim>();
    }
}
