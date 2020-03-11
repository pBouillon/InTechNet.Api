using Microsoft.AspNet.Identity.EntityFramework;

namespace InTechNet.Service.Authentication.Models.User
{
    public class ApplicationUser : IdentityUser
    {
        public string Nickname { get; set; }

        public string Password { get; set; }
    }
}
