using System;

namespace DevSpector.Application
{
    /// <summary>
    /// This is the DTO object needed to provide information from HTTP request to controller's action methods
    /// Defines contract between clients and server
    /// </summary>
    public class DeviceInfo
    {
        public string InventoryNumber { get; set; }

        public Guid TypeID { get; set; }

        public string NetworkName { get; set; }

        public string ModelName { get; set; }
    }
}
