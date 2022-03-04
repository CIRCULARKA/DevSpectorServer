using System;

namespace DevSpector.Application
{
    /// <summary>
    /// This is the DTO object needed to provide information from HTTP request to controller's action methods
    /// defining contract between clients and server
    /// </summary>
    public class UserInfo
    {
        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string Patronymic { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public Guid GroupID { get; set; }
    }
}
