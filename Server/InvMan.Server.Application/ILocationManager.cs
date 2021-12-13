using System;
using System.Collections.Generic;

namespace InvMan.Server.Application
{
	public interface ILocationManager
	{
		IEnumerable<string> Housings { get; }

		IEnumerable<string> GetCabinets(Guid housingID);
	}
}
