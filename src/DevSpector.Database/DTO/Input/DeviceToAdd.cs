using System;
using System.ComponentModel.DataAnnotations;

namespace DevSpector.Database.DTO
{
    /// <summary>
    /// This is the DTO object needed to provide information from HTTP request to controller's action methods
    /// Defines contract between clients and server
    /// </summary>
    public class DeviceToAdd
    {
        [StringLength(70, ErrorMessage = "длина инвентарного номера должна быть между {2} и {1} символами", MinimumLength = 3)]
        [Required(ErrorMessage = "инвентарный номер должен быть указан", AllowEmptyStrings = false)]
        public string InventoryNumber { get; set; }

        [Required(ErrorMessage = "идентификатор типа не был передан", AllowEmptyStrings = false)]
        public Guid TypeID { get; set; }

        [StringLength(50, ErrorMessage = "длина сетевого имени должна быть между {2} и {1} символами", MinimumLength = 3)]
        public string NetworkName { get; set; }

        [StringLength(100, ErrorMessage = "длина названия должна быть между {2} и {1} символами", MinimumLength = 1)]
        public string ModelName { get; set; }
    }
}
