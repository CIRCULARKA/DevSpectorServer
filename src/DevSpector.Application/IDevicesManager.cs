using System;
using System.Collections.Generic;
using DevSpector.SDK.Models;
using DevSpector.Domain.Models;

namespace DevSpector.Application
{
	public interface IDevicesManager
	{
		void CreateDevice(Device device);

		void UpdateDevice(Device device);

		Device GetDeviceByID(Guid deviceID);

		Cabinet GetDeviceCabinet(Guid deviceID);

		IEnumerable<Device> GetDevices();

		IEnumerable<Appliance> GetAppliances();

		IEnumerable<DeviceType> GetDeviceTypes();
	}
}
