using System.Linq;
using InvMan.Server.Domain.Models;

namespace InvMan.Server.Domain
{
	public interface ILocationRepository
	{
		IQueryable<Housing> Housings { get; }

		IQueryable<Location> Locations { get; }
	}
}
