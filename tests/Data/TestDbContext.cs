using System.Linq;
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

        private List<DeviceCabinet> _deviceCabinets;

        private List<IPAddress> _ipAddresses;

        private List<Housing> _housings;

        private List<Cabinet> _cabinets;

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
            InitializeCabinets();
            AssignCabinetsToDevices();
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

            // Add some free IPs
            for (int i = 0; i < 25; i++)
            {
                _ipAddresses.Add(new IPAddress {
                    Address = $"198.62.13.{i + 1}",
                });

                this.IPAddresses.Add(_ipAddresses[i]);
            }

            this.SaveChanges();
        }

        private void InitializeCabinets()
        {
            _housings = new List<Housing>();
            _cabinets = new List<Cabinet>();

            var lastCabinetIndex = 0;
            for (int i = 0; i < 2; i++)
            {
                _housings.Add(new Housing {
                    Name = $"TestHousing_{i + 1}"
                });

                this.Housings.Add(_housings[i]);

                this.SaveChanges();

                for (int j = 0; j < 5; j++)
                {
                    _cabinets.Add(new Cabinet {
                        Name = $"TestCabinet_{lastCabinetIndex + 1}",
                        HousingID = _housings[i].ID,
                    });

                    this.Cabinets.Add(_cabinets[lastCabinetIndex]);
                    this.SaveChanges();

                    ++lastCabinetIndex;
                }
            }

            var naHousing = new Housing {
                Name = "N/A"
            };

            _housings.Add(naHousing);
            this.Housings.Add(naHousing);
            this.SaveChanges();

            var naCabinet = new Cabinet {
                Name = "N/A",
                HousingID = naHousing.ID
            };
            _cabinets.Add(naCabinet);
            this.Cabinets.Add(naCabinet);
            this.SaveChanges();
        }

        private void AssignCabinetsToDevices()
        {
            _deviceCabinets = new List<DeviceCabinet>();

            for (int i = 0; i < _cabinets.Count - 1; i++)
            {
                _deviceCabinets.Add(new DeviceCabinet {
                    DeviceID = _devices[i].ID,
                    CabinetID = _cabinets[i].ID
                });

                this.DeviceCabinets.Add(_deviceCabinets[i]);
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
