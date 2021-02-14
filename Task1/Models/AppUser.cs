using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1.Models
{
    public class AppUser:IdentityUser
    {
        public AppUser()
        {

        }
        public AppUser(string userName) : base(userName)
        {

        }
    }
}
