using System.Linq;

namespace InvMan.Server.Application
{
	public interface ILocationManager
	{
		IQueryable<string> Housings { get; }

		IQueryable<string> GetCabinets(int housingID);
	}
}
