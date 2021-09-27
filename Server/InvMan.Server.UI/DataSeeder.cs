using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using InvMan.Server.Domain.Models;
using InvMan.Server.Database;

namespace Microsoft.AspNetCore.Builder
{
	public static class DataSeederExtension
	{
		private static ApplicationDbContext _context;

		public static IApplicationBuilder EnsurePopulated(this IApplicationBuilder app)
		{
			_context = app.ApplicationServices.
				CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();

			if (!_context.Housings.Any())
				_context.Housings.AddRange(CreateInitialHousings());

			if (!_context.Cabinets.Any())
				_context.Cabinets.AddRange(CreateInitialCabinets());

			if (!_context.DeviceTypes.Any())
				_context.DeviceTypes.AddRange(CreateIntialDeviceTypes());

			if (!_context.IPAddresses.Any())
				_context.IPAddresses.AddRange(CreateInitialIPs());

			_context.SaveChanges();

			if (!_context.HousingCabinets.Any())
				_context.HousingCabinets.AddRange(CreateInitialHousingCabinets());

			_context.SaveChanges();

			if (!_context.Locations.Any())
				_context.Locations.AddRange(CreateInitialLocations());

			_context.SaveChanges();

			if (!_context.Devices.Any())
				_context.Devices.AddRange(CreateInitialDevices());
			_context.SaveChanges();

			return app;
		}

		public static IEnumerable<DeviceType> CreateIntialDeviceTypes() =>
			new List<DeviceType> {
				new DeviceType { Name = "Персональный компьютер" },
				new DeviceType { Name = "Сервер" },
				new DeviceType { Name = "Коммутатор" }
			};

		public static IEnumerable<Device> CreateInitialDevices() =>
			new List<Device> {
				new Device {
					InventoryNumber = "NSGK530923",
					NetworkName = "IVAN-PC",
					Type = _context.DeviceTypes.First(dt => dt.ID == 1),
					Location = _context.Locations.First(l => l.ID == 1),
					IPAddresses = _context.IPAddresses.Where(ip => ip.ID == 1).ToList()
				},
				new Device {
					InventoryNumber = "NSGK654212",
					NetworkName = "MAIN-SERVER",
					Type = _context.DeviceTypes.First(dt => dt.ID == 2),
					Location = _context.Locations.First(l => l.ID == 2),
					IPAddresses = _context.IPAddresses.Where(ip => ip.ID >= 2 && ip.ID <= 5).ToList()
				},
				new Device {
					InventoryNumber = "NSGK1235231",
					NetworkName = "COMMUTATOR-1",
					Type = _context.DeviceTypes.First(dt => dt.ID == 3),
					Location = _context.Locations.First(l => l.ID == 3),
					IPAddresses = _context.IPAddresses.Where(ip => ip.ID == 6).ToList()
				}
			};

		public static IEnumerable<Housing> CreateInitialHousings() =>
			new List<Housing> {
				new Housing { Name = "N/A" },
				new Housing { Name = "Главный" },
				new Housing { Name = "Второй" }
			};

		public static IEnumerable<Cabinet> CreateInitialCabinets() =>
			new List<Cabinet> {
				new Cabinet { Name = "N/A" },
				new Cabinet { Name = "1" },
				new Cabinet { Name = "2" },
				new Cabinet { Name = "3" },
				new Cabinet { Name = "4" },
				new Cabinet { Name = "5" },
				new Cabinet { Name = "6" },
				new Cabinet { Name = "7" },
				new Cabinet { Name = "8" },
				new Cabinet { Name = "9" },
				new Cabinet { Name = "10" },
				new Cabinet { Name = "11" },
				new Cabinet { Name = "12" }
			};

		public static IEnumerable<HousingCabinets> CreateInitialHousingCabinets() =>
			new List<HousingCabinets>() {
				new HousingCabinets {
					Housing = _context.Housings.Find(1),
					Cabinets = new List<Cabinet> {
						_context.Cabinets.First(c => c.Name == "N/A")
					}
				},
				new HousingCabinets {
					Housing = _context.Housings.Find(2),
					Cabinets = _context.Cabinets.Where(c => c.ID >= 2 && c.ID <= 6).ToList()
				},
				new HousingCabinets {
					Housing = _context.Housings.Find(3),
					// Also include cabinet N/A with ID 3
					Cabinets =
						_context.Cabinets.Where(
							c =>
								(c.ID >= 7 && c.ID <= 15 && c.Name != "N/A") ||
								c.Name == "N/A").ToList()
				}
			};

		public static IEnumerable<Location> CreateInitialLocations() =>
			new List<Location> {
				new Location {
					Housing = _context.Housings.First(h => h.Name == "N/A"),
					Cabinet = _context.Cabinets.First(c => c.Name == "N/A")
				},
				new Location {
					Housing = _context.Housings.First(h => h.ID == 2),
					Cabinet =
						_context.HousingCabinets.First(hc => hc.Housing.ID == 2).Cabinets.ToList()[0]
				},
				new Location {
					Housing = _context.Housings.First(h => h.ID == 3),
					Cabinet =
						_context.HousingCabinets.First(hc => hc.Housing.ID == 3).Cabinets.ToList()[0]
				},
			};

		public static IEnumerable<IPAddress> CreateInitialIPs() =>
			new List<IPAddress> {
				new IPAddress { Address = "198.22.33.1" },
				new IPAddress { Address = "198.22.33.2" },
				new IPAddress { Address = "198.22.33.3" },
				new IPAddress { Address = "198.22.33.4" },
				new IPAddress { Address = "198.22.33.5" },
				new IPAddress { Address = "198.22.33.6" },
				new IPAddress { Address = "198.22.33.7" },
				new IPAddress { Address = "198.22.33.8" },
				new IPAddress { Address = "198.22.33.9" },
				new IPAddress { Address = "198.22.33.10" },
				new IPAddress { Address = "198.22.33.11" },
				new IPAddress { Address = "198.22.33.12" },
				new IPAddress { Address = "198.22.33.13" }
			};
	}
}
