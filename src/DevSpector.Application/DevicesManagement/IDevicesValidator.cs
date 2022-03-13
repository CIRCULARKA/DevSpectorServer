using System;

namespace DevSpector.Application.Devices
{
    public interface IDevicesValidator
    {
        bool DoesDeviceExists(string inventoryNumber);

        bool DoesDeviceTypeExists(Guid typeID);
    }
}
