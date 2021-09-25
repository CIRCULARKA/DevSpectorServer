using System.Collections.Generic;
using InvMan.Server.Domain.Models;

namespace InvMan.Server.Domain
{
	public interface ILocationRepository
	{
		IEnumerable<Housing> Housings { get; }

		IEnumerable<Cabinet> GetHousingCabinets(int cabinetId);
	}
}
