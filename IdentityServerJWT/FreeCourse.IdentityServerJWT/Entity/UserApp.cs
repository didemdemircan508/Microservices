using Microsoft.AspNetCore.Identity;

namespace FreeCourse.IdentityServerJWT.Entity
{
    public class UserApp: IdentityUser
    {
        public string? City { get; set; }
    }
}
