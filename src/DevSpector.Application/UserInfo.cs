namespace DevSpector.Application
{
    /// <summary>
    /// This is the DTO object needed to provide information from HTTP request to controller's action methods
    /// defining contract between clients and server
    /// </summary>
    public class UserInfo
    {
        string FirstName { get; set; }

        string Surname { get; set; }

        string Patronymic { get; set; }

        string Login { get; set; }

        string Password { get; set; }
    }
}
