using System.ComponentModel.DataAnnotations;

namespace DevSpector.Database.DTO
{
    public class LANInfo
    {
        [Required(ErrorMessage = "сетевой адрес не был передан", AllowEmptyStrings = false)]
        public string NetworkAddress { get; set; }

        [Required(ErrorMessage = "маска подсети не была передана")]
        public int Mask { get; set; }
    }
}
