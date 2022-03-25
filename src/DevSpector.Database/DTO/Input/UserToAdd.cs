using System;

namespace DevSpector.Database.DTO
{
    /// <summary>
    /// This is the DTO object needed to provide information from HTTP request to controller's action methods
    /// defining contract between clients and server
    /// </summary>
    public class UserToAdd
    {
        public string FirstName { get; init; }

        public string Surname { get; init; }

        public string Patronymic { get; init; }

        public string Login { get; init; }

        public string Password { get; init; }

        public string AccessToken { get; init; }

        public Guid GroupID { get; init; }
    }
}
