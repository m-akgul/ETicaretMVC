using System;
using Microsoft.AspNetCore.Identity;

namespace ETicaretData.Identity
{
    public class AppRole : IdentityRole<int>
    {

        public AppRole() : base()
        {
        }

        public AppRole(string roleName) : base(roleName)
        {
        }
    }
}

