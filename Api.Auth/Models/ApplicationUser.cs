using Microsoft.AspNetCore.Identity;

namespace Api.Auth.Models
{
    /// <summary>
    /// ApplicationUser extends IdentityUser to add custom properties.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
    }
}
