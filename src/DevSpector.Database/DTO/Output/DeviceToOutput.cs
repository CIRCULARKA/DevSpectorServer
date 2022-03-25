using System;
using System.Collections.Generic;

namespace DevSpector.Database.DTO
{
	public class DeviceToOutput
	{
		public Guid ID { get; init; }

		public string InventoryNumber { get; init; }

		public string Type { get; init; }

		public string NetworkName { get; init; }

		public string Housing { get; init; }

		public string Cabinet { get; init; }

		public List<string> IPAddresses { get; init; }

		public List<string> Software { get; init; }
	}
}
