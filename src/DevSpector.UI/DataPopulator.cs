using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DevSpector.Database;
using DevSpector.Application;
using DevSpector.Domain.Models;

namespace Microsoft.AspNetCore.Builder
{
    public static class DataPopulator
    {
        public static IApplicationBuilder AddUserGroups(
            this IApplicationBuilder @this,
            ApplicationDbContext context
        )
        {
            var roles = context.Roles;
            context.Roles.RemoveRange(roles);

            context.Roles.AddRange(
                new IdentityRole("Техник") { NormalizedName = "ТЕХНИК" },
                new IdentityRole("Администратор") { NormalizedName = "АДМИНИСТРАТОР" }
            );

            context.SaveChanges();

            return @this;
        }

        public static IApplicationBuilder AddRootUser(
            this IApplicationBuilder @this,
            ClientUsersManager context
        )
        {
            if (context.FindByNameAsync("root").Result != null)
                return @this;

            var rootUser = new ClientUser {
                UserName = "root",
                Group = "Администратор", // Administrator
                AccessKey = Guid.NewGuid().ToString()
            };

            var creationResult = context.CreateAsync(rootUser, Environment.GetEnvironmentVariable("ROOT_PWD")).Result;
            var roleResult = context.AddToRoleAsync(rootUser, rootUser.Group).Result;

            return @this;
        }

        public static IApplicationBuilder FillDbWithTemporaryData(
            this IApplicationBuilder @this,
            ApplicationDbContext context,
            ClientUsersManager usersManager
        )
        {
            if (usersManager.FindByNameAsync("root").GetAwaiter().GetResult() == null) {
                var group = "Администратор"; // Administrator
                var root = new ClientUser {
                    AccessKey = Guid.NewGuid().ToString(),
                    UserName = "root",
                    Group = group
                };

                usersManager.CreateAsync(root, "123Abc!").GetAwaiter().GetResult();
                usersManager.AddToRoleAsync(root, group).GetAwaiter().GetResult();
            }

            if (context.Devices.Count() != 0) return @this;

            // context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var housings = new List<Housing>() {
				new Housing { ID = Guid.NewGuid(), Name = "N/A" },
				new Housing { ID = Guid.NewGuid(), Name = "Главный" },
				new Housing { ID = Guid.NewGuid(), Name = "Второй" }
            };

            context.Housings.AddRange(housings);
            context.SaveChanges();

 			var cabinets = new List<Cabinet>();

            var locations = new List<Location>();

            for (int j = 0; j < 4; j++)
            {
                for (int i = 1; i <= 12; i++)
                {
                    var newCabinet = new Cabinet { ID = Guid.NewGuid(), Name = $"{(j == 0 ? "" : j.ToString()) + i.ToString()}" };
                    context.Cabinets.Add(newCabinet);
                    context.SaveChanges();
                    locations.Add(new Location { ID = Guid.NewGuid(), CabinetID = newCabinet.ID, HousingID = housings[1].ID });
                }
            }

            for (int j = 0; j < 3; j++)
            {
                for (int i = 1; i <= 12; i++)
                {
                    var newCabinet = new Cabinet { ID = Guid.NewGuid(), Name = $"{(j == 0 ? "" : j.ToString()) + i.ToString()}" };
                    context.Cabinets.Add(newCabinet);
                    context.SaveChanges();
                    locations.Add(new Location { ID = Guid.NewGuid(), CabinetID = newCabinet.ID, HousingID = housings[2].ID });
                }
            }

            var naCabinet = new Cabinet { ID = Guid.NewGuid(), Name = "N/A " };
            context.Cabinets.Add(naCabinet);
            context.SaveChanges();

            locations.Add(
                new Location { ID = Guid.NewGuid(), CabinetID = naCabinet.ID, HousingID = housings[0].ID }
            );

            context.Locations.AddRange(locations);
            context.SaveChanges();

            var deviceTypes = new List<DeviceType>();

            deviceTypes.Add(new DeviceType { ID = Guid.NewGuid(), Name = "Персональный компьютер" });
            deviceTypes.Add(new DeviceType { ID = Guid.NewGuid(), Name = "Коммутатор" });
            deviceTypes.Add(new DeviceType { ID = Guid.NewGuid(), Name = "Сервер" });
            deviceTypes.Add(new DeviceType { ID = Guid.NewGuid(), Name = "Камера" });
            deviceTypes.Add(new DeviceType { ID = Guid.NewGuid(), Name = "Принтер" });

            context.DeviceTypes.AddRange(deviceTypes);
            context.SaveChanges();

            var devices = new List<Device>();

            var locationIterator = 0;
            foreach (var type in deviceTypes)
                for (int i = 1; i <= 5; i++, locationIterator++)
                    devices.Add(
                        new Device {
                            ID = Guid.NewGuid(),
                            NetworkName = $"TempNetworkName:{Guid.NewGuid()}",
                            InventoryNumber = $"TempInventoryNumber:{Guid.NewGuid()}",
                            LocationID = locations[locationIterator].ID,
                            TypeID = type.ID
                        }
                    );

            context.Devices.AddRange(devices);
            context.SaveChanges();

            var ipAddresses = new List<IPAddress>();

            for (int i = 0, j = -1; i <= 255; i++)
            {
                // 10 addresses for each device
                if (i % 11 == 0) j++;
                ipAddresses.Add(new IPAddress { ID = Guid.NewGuid(), Address = $"198.33.12.{i}", DeviceID = devices[j].ID });
            }

            // Some free IP's
            for (int i = 0; i < 100; i++)
            {
                ipAddresses.Add(
                    new IPAddress {
                        ID = Guid.NewGuid(),
                        Address = $"198.33.{13 + (i / 10)}.{i}"
                    });
            }

            context.IPAddresses.AddRange(ipAddresses);
            context.SaveChanges();

            return @this;
        }
    }
}
