using System;

namespace DevSpector.Application.Devices
{
    public interface IDevicesValidator
    {
        bool IsInventoryNumberUnique(string inventoryNumber);

        bool DoesDeviceTypeExists(Guid typeID);

        bool IsNetworkNameUnique(string networkName);
    }
}
