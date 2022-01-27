using System.Threading.Tasks;

namespace InvMan.Common.SDK
{
	public interface IDevicesModifier
	{
		Task CreateDevice(string networkName, string inventoryNumber, string type);
	}
}
