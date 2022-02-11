using Microsoft.AspNetCore.Identity;

namespace InvMan.Server.Domain.Models
{
    public class ClientUser : IdentityUser
    {
        public string AccessKey { get; set; }

        public string Group { get; set; }
    }
}
