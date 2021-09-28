using System.Linq;
using InvMan.Common.SDK.Models;
using InvMan.Server.Domain.Models;

namespace InvMan.Server.Application
{
	public interface IDevicesManager
	{
		Device GetDeviceByID(int deviceID);

		IQueryable<Device> GetDevices(int amount);

		IQueryable<Appliance> GetAppliances(int amount);
	}
}
