using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using DevSpector.Database;
using DevSpector.Database.DTO;
using DevSpector.Application;
using DevSpector.Domain.Models;

namespace Microsoft.AspNetCore.Builder
{
    public static class DataPopulator
    {
        public static IApplicationBuilder AddUserGroup(
            this IApplicationBuilder @this,
            string groupName
        )
        {
            var context = GetService<ApplicationContextBase>(@this);

            var roles = context.Roles;

            if (roles.FirstOrDefault(r => r.NormalizedName == groupName.ToUpper()) != null)
                return @this;

            context.Roles.AddRange(
                new IdentityRole(groupName) { NormalizedName = groupName.ToUpper() }
            );

            context.SaveChanges();

            return @this;
        }

        public async static Task<IApplicationBuilder> AddSuperUserAsync(
            this IApplicationBuilder @this,
            string login,
            string password
        )
        {
            var context = GetService<UsersManager>(@this);

            if ((await context.FindByLoginAsync(login)) != null)
                return @this;

            var administratorGroup = context.GetGroup("Суперпользователь");
            var newUserToAdd = new UserToAdd {
                Login = "root",
                Password = password,
                GroupID = new Guid(administratorGroup.Id)
            };

            await context.CreateUserAsync(newUserToAdd);

            return @this;
        }

        public static IApplicationBuilder FillDbWithTemporaryDataAsync(
            this IApplicationBuilder @this
        )
        {
            var context = GetService<ApplicationContextBase>(@this);
            var usersManager = GetService<UsersManager>(@this);

            if (context.Devices.Count() != 0) return @this;

            var housings = new List<Housing>() {
				new Housing { ID = Guid.NewGuid(), Name = "N/A" },
				new Housing { ID = Guid.NewGuid(), Name = "Главный" },
				new Housing { ID = Guid.NewGuid(), Name = "Второй" }
            };

            context.Housings.AddRange(housings);
            context.SaveChanges();

 			var cabinets = new List<Cabinet>();

            // Add cabinets to "Главный" (Main) housing
            for (int j = 0; j < 4; j++)
            {
                for (int i = 1; i <= 12; i++)
                {
                    var newCabinet = new Cabinet { ID = Guid.NewGuid(), Name = $"{(j == 0 ? "" : j.ToString()) + i.ToString()}" };
                    newCabinet.HousingID = housings[1].ID;
                    context.Cabinets.Add(newCabinet);
                    context.SaveChanges();
                }
            }

            // Add cabinets to "Второй" (Secondary) housing
            for (int j = 0; j < 3; j++)
            {
                for (int i = 1; i <= 12; i++)
                {
                    var newCabinet = new Cabinet { ID = Guid.NewGuid(), Name = $"{(j == 0 ? "" : j.ToString()) + i.ToString()}" };
                    newCabinet.HousingID = housings[2].ID;
                    context.Cabinets.Add(newCabinet);
                    context.SaveChanges();
                }
            }

            // Add N/A cabinet with N/A housing
            var naCabinet = new Cabinet { ID = Guid.NewGuid(), Name = "N/A" };
            naCabinet.HousingID = housings[0].ID;
            context.Cabinets.Add(naCabinet);
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
                            ModelName = $"TempModelName:{Guid.NewGuid()}",
                            TypeID = type.ID
                        }
                    );

            context.Devices.AddRange(devices);
            context.SaveChanges();

            // Assign N/A location for each device
            foreach (var device in devices)
                context.DeviceCabinets.Add(
                    new DeviceCabinet { DeviceID = device.ID, CabinetID = naCabinet.ID }
                );
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

            // Add 20 software to each device
            foreach (var device in devices)
            {
                for (int i = 0; i < 20; i++)
                {
                    context.DeviceSoftware.Add(new DeviceSoftware {
                        DeviceID = device.ID,
                        SoftwareName = $"TempSoftwareName:{Guid.NewGuid().ToString()}",
                        SoftwareVersion = $"TempSoftwareVersion:{Guid.NewGuid().ToString()}"
                    });
                }
            }

            context.SaveChanges();

            return @this;
        }

        private static T GetService<T>(IApplicationBuilder builder) =>
            builder.ApplicationServices.
                CreateScope().
                ServiceProvider.
                GetService<T>();
    }
}
