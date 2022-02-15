using System;
using System.Collections.Generic;
using DevSpector.SDK.Models;
using DevSpector.Domain.Models;

namespace DevSpector.Application
{
	public interface IDevicesManager
	{
		void CreateDevice(string networkName, string inventoryNumber, string type);

		Device GetDeviceByID(Guid deviceID);

		IEnumerable<Device> GetDevices();

		IEnumerable<Appliance> GetAppliances();
	}
}
