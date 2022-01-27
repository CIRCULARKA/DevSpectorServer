using System.Linq;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using ReactiveUI;
using InvMan.Desktop.Service;
using InvMan.Common.SDK.Models;

namespace InvMan.Desktop.UI.ViewModels
{
    public class SearchViewModel : ViewModelBase, ISearchViewModel
    {
        private string _searchQuery;

        private IApplicationEvents _events;

        private IDevicesListViewModel _devicesListViewModel;

        public SearchViewModel(
            IApplicationEvents events,
            IDevicesListViewModel devicesListViewModel
        )
        {
            _events = events;
            _devicesListViewModel = devicesListViewModel;

            FilterDevicesCommand = ReactiveCommand.Create(
                async () => {
                    try
                    {
                        devicesListViewModel.AreAppliancesLoaded = false;
                        devicesListViewModel.AreThereAppliances = false;
                        events.RaiseSearchExecuted(
                            await FilterDevicesAsync(devicesListViewModel.CachedDevices)
                        );
                    }
                    finally { devicesListViewModel.AreAppliancesLoaded = true; }
                }
            );
        }

        public string SearchQuery
        {
            get => _searchQuery;
            set => this.RaiseAndSetIfChanged(ref _searchQuery, value);
        }

        public ReactiveCommand<Unit, Task> FilterDevicesCommand { get; }

        private Task<IEnumerable<Appliance>> FilterDevicesAsync(IEnumerable<Appliance> devices)
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
                    return Task.FromResult(new List<Appliance>(devices).AsEnumerable());

            var result = new List<Appliance>(devices.Count());

            var filteringTask = Task.Run(
                () => {
                    result.AddRange(devices.Where(d => d.NetworkName.Contains(SearchQuery)));
                    result.AddRange(devices.Where(d => d.InventoryNumber.Contains(SearchQuery)));
                    result.AddRange(devices.Where(d => d.Housing.Contains(SearchQuery)));
                    result.AddRange(devices.Where(d => d.Cabinet.Contains(SearchQuery)));
                    result.AddRange(devices.Where(d => d.Type.Contains(SearchQuery)));

                    Thread.Sleep(3000);

                    return result.AsEnumerable<Appliance>();
                }
            );

            return filteringTask;
        }
    }
}
