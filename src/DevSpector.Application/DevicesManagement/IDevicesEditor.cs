using System;
using System.Collections.Generic;
using DevSpector.SDK.Models;
using DevSpector.Domain.Models;
using DevSpector.Database;
using DevSpector.Application.Enumerations;

namespace DevSpector.Application
{
	public interface IDevicesEditor
	{
		void CreateDevice(DeviceInfo info);

		void UpdateDevice(string targetDeviceInventoryNumber, DeviceInfo info);

		void DeleteDevice(string inventoryNumber);

		void MoveDevice(string inventoryNumber, Guid cabinetID);

		void AddSoftware(string inventoryNumber, SoftwareInfo info);

		void RemoveSoftware(string inventoryNumber, SoftwareInfo info);

		void AddIPAddress(string inventoryNumber, string ipAddress);

		void RemoveIPAddress(string inventoryNumber, string ipAddress);
	}
}