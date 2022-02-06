using Microsoft.AspNetCore.Identity;

namespace InvMan.Server.Domain.Models
{
    public class ClientUser : IdentityUser
    {
        public string AccessKey { get; init; }

        public string Group { get; init; }
    }
}
