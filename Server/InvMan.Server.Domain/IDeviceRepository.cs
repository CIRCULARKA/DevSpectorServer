using System.Threading.Tasks;
using System.Collections.Generic;
using InvMan.Server.Domain.Models;

namespace InvMan.Server.Domain
{
	public interface IDeviceRepository
	{
		Task<int> CreateDevice(Device newDevice);

		Task<int> RemoveDevice(int deviceID);

		Task<int> UpdateDevice(Device newDevice);

		Device GetDeviceByID(int deviceID);

		Device GetDeviceByInventoryNumber(string invNum);

		IEnumerable<Device> AllDevices { get; }
	}
}
