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
using DevSpector.Application.Networking;
using DevSpector.Application.Devices;

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
            var ipEditor = GetService<IIPAddressEditor>(@this);

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

            ipEditor.GenerateRange("198.22.33.1", 24);

            List<IPAddress> ipAddresses = context.IPAddresses.ToList();

            // Add 10 ips to each device
            var ipIterator = 0;
            for (int i = 0; i < devices.Count; i++)
            {
                for (int j = 0; j < 10; j++, ipIterator++)
                {
                    context.DeviceIPAddresses.Add(new DeviceIPAddress {
                        DeviceID = devices[i].ID,
                        IPAddressID = ipAddresses[ipIterator].ID
                    });
                }
            }

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
