using Microsoft.AspNetCore.Identity;

namespace Savoy.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool RememberMe { get; set; } = false;
    }
}
