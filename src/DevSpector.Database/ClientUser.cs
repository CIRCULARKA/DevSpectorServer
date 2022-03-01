using Microsoft.AspNetCore.Identity;

namespace DevSpector.Domain.Models
{
    public class ClientUser : IdentityUser
    {
        public string AccessKey { get; set; }
    }
}
