using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using DevSpector.Database;
using DevSpector.Database.DTO;
using DevSpector.Application;
using DevSpector.Domain.Models;
using DevSpector.Application.Networking;

namespace Microsoft.AspNetCore.Builder
{
    public static class DataPopulator
    {
        public static IApplicationBuilder InitializeData(
            this IApplicationBuilder @this,
            string superUserLogin,
            string superUserPassword
        )
        {
            var context = GetService<ApplicationContextBase>(@this);
            var usersManager = GetService<UsersManager>(@this);
            var ipGenerator = GetService<IIPRangeGenerator>(@this);

            //
            // Create user groups
            //
            List<IdentityRole> roles = CreateUserGroups("Техник", "Администратор", "Суперпользователь");
            context.Roles.AddRange(roles);
            context.SaveChanges();
            //

            //
            // Create super user
            //
            var rootUser = CreateUser(
                login: superUserLogin,
                password: superUserPassword,
                role: roles.FirstOrDefault(r => r.NormalizedName == "СУПЕРПОЛЬЗОВАТЕЛЬ")
            );
            usersManager.CreateUserAsync(rootUser).GetAwaiter().GetResult();
            //

            //
            // Create housings
            //
            List<Housing> housings = CreateHousings("N/A", "Главный", "Второй");
            context.Housings.AddRange(housings);
            context.SaveChanges();
            //

            //
            // Add cabinets to housings
            //
            List<Cabinet> naCabinet = CreateCabinets(
                housing: housings.FirstOrDefault(h => h.Name == "N/A"),
                cabs: "N/A"
            );

            List<Cabinet> mainCabinets = CreateCabinets(
                housing: housings.FirstOrDefault(h => h.Name == "Главный"),
                // ДОПОЛНИТЬ
                "0", "1"
            );

            List<Cabinet> secondaryCabinets = CreateCabinets(
                housing: housings.FirstOrDefault(h => h.Name == "Второй"),
                // ДОПОЛНИТЬ
                "0", "1"
            );

            context.Cabinets.AddRange(naCabinet);
            context.Cabinets.AddRange(mainCabinets);
            context.Cabinets.AddRange(secondaryCabinets);
            context.SaveChanges();
            ///

            //
            // Create device types
            //
            List<DeviceType> deviceTypes = CreateDeviceTypes(
                "Персональный компьютер",
                "Сервер",
                "Коммутатор",
                "Принтер",
                "Камера"
            );

            //
            // Set IP range
            //
            List<IPAddress> ips = CreateIPAddresses(
                ipGenerator.GenerateRange("192.168.0.100", 24).ToList()
            );
            context.IPAddresses.AddRange(ips);
            context.SaveChanges();

            return @this;
        }

        private static List<IdentityRole> CreateUserGroups(params string[] groups)
        {
            var result = new List<IdentityRole>(capacity: groups.Length);

            foreach (var group in groups)
                result.Add(new IdentityRole(group) { NormalizedName = group.ToUpper() });

            return result;
        }

        private static UserToAdd CreateUser(string login, string password, IdentityRole role) =>
            new UserToAdd {
                Login = login,
                Password = password,
                GroupID = new Guid(role.Id)
            };

        private static List<Housing> CreateHousings(params string[] names)
        {
            var result = new List<Housing>(capacity: names.Length);

            foreach (var name in names)
                result.Add(new Housing { Name = name });

            return result;
        }

        private static List<Cabinet> CreateCabinets(Housing housing, params string[] cabs)
        {
            var result = new List<Cabinet>(capacity: cabs.Length);

            foreach (var cab in cabs)
                result.Add(new Cabinet { HousingID = housing.ID, Name = cab });

            return result;
        }

        private static List<DeviceType> CreateDeviceTypes(
            params string[] types
        )
        {
            var result = new List<DeviceType>(capacity: types.Length);

            foreach (var type in types)
                result.Add(new DeviceType { Name = type });

            return result;
        }

        private static List<IPAddress> CreateIPAddresses(List<string> ips)
        {
            var result = new List<IPAddress>(capacity: ips.Count);

            foreach (var ip in ips)
                result.Add(new IPAddress { Address = ip });

            return result;
        }

        private static T GetService<T>(IApplicationBuilder builder) =>
            builder.ApplicationServices.
                CreateScope().
                ServiceProvider.
                GetService<T>();
    }
}
