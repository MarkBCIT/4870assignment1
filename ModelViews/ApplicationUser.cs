using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace assignment.ModelViews
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string Country { set; get; }
        public string MobileNumber { set; get; }
        
    }
}
