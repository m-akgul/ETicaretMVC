using System;
using Microsoft.AspNetCore.Identity;

namespace ETicaretData.Identity
{
    public class AppUser : IdentityUser<int>
    {

        public AppUser() : base()
        {
        }

        public AppUser(string userName) : base(userName)
        {
        }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? AddressTitle1 { get; set; }
        public string? Address1 { get; set; }
        public string? City1 { get; set; }
        public string? AddressTitle2 { get; set; }
        public string? Address2 { get; set; }
        public string? City2 { get; set; }
    }
}

