using System;

namespace DevSpector.Database.DTO
{
    /// <summary>
    /// This is the DTO object needed to provide information from HTTP request to controller's action methods
    /// Defines contract between clients and server
    /// </summary>
    public class DeviceToAdd
    {
        public string InventoryNumber { get; set; }

        public Guid TypeID { get; set; }

        public string NetworkName { get; set; }

        public string ModelName { get; set; }
    }
}
