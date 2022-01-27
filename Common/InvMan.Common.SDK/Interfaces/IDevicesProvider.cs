using System.Threading.Tasks;
using System.Collections.Generic;
using InvMan.Common.SDK.Models;

namespace InvMan.Common.SDK
{
	public interface IDevicesProvider
	{
		Task<IEnumerable<Appliance>> GetDevicesAsync();
	}
}
