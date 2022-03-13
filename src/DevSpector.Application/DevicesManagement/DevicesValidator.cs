using System;
using DevSpector.Domain;
using DevSpector.Domain.Models;

namespace DevSpector.Application.Devices
{
    public class DevicesValidator : IDevicesValidator
    {
        private readonly IRepository _repo;

        public DevicesValidator(IRepository repo)
        {
            _repo = repo;
        }

        public bool DoesDeviceExists(string inventoryNumber)
        {
			var existingDevice = _repo.GetSingle<Device>(d => d.InventoryNumber == inventoryNumber);
            return existingDevice != null;
        }

        public bool DoesDeviceTypeExists(Guid typeID) =>
			_repo.GetByID<DeviceType>(typeID) != null;
    }
}
