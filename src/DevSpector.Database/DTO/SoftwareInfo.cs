using System.ComponentModel.DataAnnotations;

namespace DevSpector.Database.DTO
{
    /// <summary>
    /// This is the DTO object needed to provide information from HTTP request to controller's action methods
    /// Defines contract between clients and server
    /// </summary>
    public class SoftwareInfo
    {
        [Required(ErrorMessage = "название ПО должно быть указано", AllowEmptyStrings = false)]
        [StringLength(100, ErrorMessage = "название ПО должно быть длиной между {2} и {1} символов", MinimumLength = 1)]
        public string SoftwareName { get; set; }

        [StringLength(50, ErrorMessage = "версия ПО должно быть длиной между {2} и {1} символов", MinimumLength = 1)]
        public string SoftwareVersion { get; set; }
    }
}
