using System.Linq;
using System.Threading.Tasks;
using InvMan.Server.Domain.Models;

namespace InvMan.Server.Domain
{
	public interface IDeviceRepository
	{
		Task<int> CreateDevice(Device newDevice);

		Task<int> RemoveDevice(int deviceID);

		Task<int> UpdateDevice(Device newDevice);

		IQueryable<Device> Devices { get; }
	}
}
