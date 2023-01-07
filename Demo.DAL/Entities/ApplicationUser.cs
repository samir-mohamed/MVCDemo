using Microsoft.AspNetCore.Identity;

namespace Demo.DAL.Entities
{
    public class ApplicationUser : IdentityUser 
    {
        public bool IsAgree { get; set; }
    }
}
