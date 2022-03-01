using Microsoft.AspNetCore.Identity;

namespace DevSpector.Domain.Models
{
    public class ClientUser : IdentityUser
    {
        public string AccessKey { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Patronymic { get; set; }
    }
}
