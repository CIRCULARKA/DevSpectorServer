using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using DevSpector.Database;
using DevSpector.Domain.Models;

namespace DevSpector.Tests.Database
{
    public class TestDbContext : ApplicationContextBase
    {
        private List<Device> _devices;

        private List<DeviceType> _deviceTypes;

        private List<DeviceSoftware> _deviceSoftware;

        private List<IPAddress> _ipAddresses;

        public TestDbContext() :
            base(new DbContextOptions<TestDbContext>())
        {
            RecreateDatabase();

            InitializeTestData();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);

            builder.UseSqlite("Data Source=./TestData.db");
        }

        private void InitializeTestData()
        {
            InitializeDeviceTypes();
            InitializeDevices();
            InitializeDeviceSoftware();
            InitializeIPAddresses();
        }

        private void InitializeDeviceTypes()
        {
            _deviceTypes = new List<DeviceType>();

            for (int i = 0; i < 5; i++)
            {
                _deviceTypes.Add(new DeviceType {
                    Name = $"TestDeviceType_{i + 1}"
                });

                this.DeviceTypes.Add(_deviceTypes[i]);
            }

            this.SaveChanges();
        }

        private void InitializeDevices()
        {
            _devices = new List<Device>();

            for (int i = 0; i < 10; i++)
            {
                _devices.Add(new Device {
                    InventoryNumber = $"TestInventoryNumber_{i + 1}",
                    NetworkName = $"TestNetworkName_{i + 1}",
                    ModelName = $"TestModelName_{i + 1}",
                    // Each 2 devices have the same type
                    TypeID = _deviceTypes[i / 2].ID
                });

                this.Devices.Add(_devices[i]);
            }

            this.SaveChanges();
        }

        private void InitializeDeviceSoftware()
        {
            _deviceSoftware = new List<DeviceSoftware>();

            for (int i = 0; i < 20; i++)
            {
                _deviceSoftware.Add(new DeviceSoftware {
                    SoftwareName = $"TestSoftwareName_{i + 1}",
                    SoftwareVersion = $"TestSoftwareVersion_{i + 1}",
                    // Each device have 2 software
                    DeviceID = _devices[i / 2].ID
                });

                this.DeviceSoftware.Add(_deviceSoftware[i]);
            }

            this.SaveChanges();
        }

        private void InitializeIPAddresses()
        {
            _ipAddresses = new List<IPAddress>();

            for (int i = 0; i < 50; i++)
            {
                _ipAddresses.Add(new IPAddress {
                    Address = $"198.62.14.{i + 1}",
                    // Each device have 5 IP's
                    DeviceID = _devices[i / 5].ID
                });

                this.IPAddresses.Add(_ipAddresses[i]);
            }

            this.SaveChanges();
        }

        private void RecreateDatabase()
        {
            this.Database.EnsureDeleted();
            this.Database.EnsureCreated();
        }
    }
}
