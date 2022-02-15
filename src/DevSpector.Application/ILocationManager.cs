using System;
using System.Collections.Generic;

namespace DevSpector.Application
{
	public interface ILocationManager
	{
		IEnumerable<string> Housings { get; }

		IEnumerable<string> GetCabinets(Guid housingID);
	}
}
