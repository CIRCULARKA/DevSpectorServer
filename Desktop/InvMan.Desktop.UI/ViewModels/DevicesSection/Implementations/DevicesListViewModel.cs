using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using InvMan.Common.SDK;
using InvMan.Desktop.Service;
using InvMan.Common.SDK.Models;
using ReactiveUI;

namespace InvMan.Desktop.UI.ViewModels
{
    public class DevicesListViewModel : ListViewModelBase<Appliance>, IDevicesListViewModel
    {
        private readonly IApplicationEvents _appEvents;

        private readonly IDevicesProvider _devicesProvider;

        private readonly IUserSession _session;

        public DevicesListViewModel(
            IDevicesProvider devicesProvider,
            IApplicationEvents appEvents,
            IUserSession session
        )
        {
            _appEvents = appEvents;
            _session = session;
            _devicesProvider = devicesProvider;
        }

        public override Appliance SelectedItem
        {
            get => _selectedItem;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedItem, value);

                _appEvents.RaiseApplianceSelected(_selectedItem);
            }
        }

        public override void LoadItemsFromList(IEnumerable<Appliance> devices)
        {
            Items.Clear();

            foreach (var device in devices)
                Items.Add(device);

            if (Items.Count == 0) {
                AreThereItems = false;
                NoItemsMessage = "Устройства не найдены";
            }
            else AreThereItems = true;
        }

        protected override async Task LoadItems()
        {
            AreItemsLoaded = false;

            ItemsCache = await _devicesProvider.GetDevicesAsync(_session.AccessToken);
            Items.Clear();
            foreach (var device in ItemsCache)
                Items.Add(device);
        }

        public override async void InitializeList()
        {
            try
            {
                await LoadItems();

                if (Items.Count > 0) {
                    AreThereItems = true;
                    SelectedItem = Items[0];
                }
                else {
                    AreThereItems = false;
                    NoItemsMessage = "Нет устройств";
                }
            }
            catch (ArgumentException)
            {
                AreThereItems = false;
                NoItemsMessage = "Ошибка доступа";
            }
            catch
            {
                AreThereItems = false;
                NoItemsMessage = "Что-то пошло не так";
            }
            finally { AreItemsLoaded = true; }
        }
    }
}
