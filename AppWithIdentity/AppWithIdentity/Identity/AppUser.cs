using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppWithIdentity.Identity
{
    public class AppUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedName { get; set; }
        public string PasswordHash { get; set; }
    }
}
