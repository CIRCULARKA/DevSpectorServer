using Microsoft.AspNetCore.Identity;

namespace InvMan.Server.Domain.Models
{
    public class DesktopUser : IdentityUser
    {
        public string AccessKey { get; init; }

        public string Group { get; init; }
    }
}
