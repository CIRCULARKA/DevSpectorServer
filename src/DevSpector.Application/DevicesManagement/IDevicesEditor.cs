using System;
using DevSpector.Database.DTO;

namespace DevSpector.Application.Devices
{
	public interface IDevicesEditor
	{
		void CreateDevice(DeviceToAdd info);

		void UpdateDevice(string targetDeviceInventoryNumber, DeviceToAdd info);

		void DeleteDevice(string inventoryNumber);

		void MoveDevice(string inventoryNumber, Guid cabinetID);

		void AddSoftware(string inventoryNumber, SoftwareInfo info);

		void RemoveSoftware(string inventoryNumber, SoftwareInfo info);

		void AddIPAddress(string inventoryNumber, string ipAddress);

		void RemoveIPAddress(string inventoryNumber, string ipAddress);
	}
}
