using System;
using System.Collections.Generic;
using DevSpector.Domain.Models;

namespace DevSpector.Application
{
	public interface ILocationManager
	{
		IEnumerable<Housing> Housings { get; }

		IEnumerable<Cabinet> GetCabinets(Guid housingID);
	}
}
