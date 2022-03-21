using System;
using System.Collections.Generic;
using DevSpector.Domain.Models;

namespace DevSpector.Application.Location
{
	public interface ILocationManager
	{
		List<Housing> Housings { get; }

		List<Cabinet> GetCabinets(Guid housingID);
	}
}
