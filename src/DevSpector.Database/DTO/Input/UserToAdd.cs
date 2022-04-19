using System;
using System.ComponentModel.DataAnnotations;

namespace DevSpector.Database.DTO
{
    /// <summary>
    /// This is the DTO object needed to provide information from HTTP request to controller's action methods
    /// defining contract between clients and server
    /// </summary>
    public class UserToAdd
    {
        [StringLength(100, ErrorMessage = "длина имени должна находиться между {2} и {1} символами", MinimumLength = 2)]
        public string FirstName { get; init; }

        [StringLength(100, ErrorMessage = "длина фамилии должна находиться между {2} и {1} символами", MinimumLength = 2)]
        public string Surname { get; init; }

        [StringLength(100, ErrorMessage = "длина отчества должна находиться между {2} и {1} символами", MinimumLength = 2)]
        public string Patronymic { get; init; }

        [Required(ErrorMessage = "логин не был предоставлен", AllowEmptyStrings = false)]
        [StringLength(100, ErrorMessage = "длина логина должна находиться между {2} и {1} символами", MinimumLength = 2)]
        public string Login { get; init; }

        [Required(ErrorMessage = "пароль не был предоставлен", AllowEmptyStrings = false)]
        [StringLength(100, ErrorMessage = "длина пароля должна находиться между {2} и {1} символами", MinimumLength = 5)]
        public string Password { get; init; }

        public Guid GroupID { get; init; }
    }
}
