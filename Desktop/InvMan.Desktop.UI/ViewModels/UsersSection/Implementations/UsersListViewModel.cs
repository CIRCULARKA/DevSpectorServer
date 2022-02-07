using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using InvMan.Common.SDK;
using InvMan.Desktop.Service;
using InvMan.Common.SDK.Models;
using ReactiveUI;

namespace InvMan.Desktop.UI.ViewModels
{
    public class UsersListViewModel : ListViewModelBase<User>, IUsersListViewModel
    {
        private readonly IApplicationEvents _appEvents;

        private readonly IUsersProvider _usersProvider;

        private readonly IUserSession _session;

        public UsersListViewModel(
            IUsersProvider usersProvider,
            IApplicationEvents appEvents,
            IUserSession session
        )
        {
            _appEvents = appEvents;
            _session = session;
            _usersProvider = usersProvider;
        }

        public override User SelectedItem
        {
            get => _selectedItem;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedItem, value);
                _appEvents.RaiseUserSelected(_selectedItem);
            }
        }

        public void LoadFromList(IEnumerable<User> devices)
        {
            Items.Clear();

            foreach (var device in devices)
                Items.Add(device);

            if (Items.Count == 0) {
                AreThereItems = false;
                NoItemsMessage = "Пользователи не найдены";
            }
            else AreThereItems = true;

        }

        protected override async Task LoadItems()
        {
            AreItemsLoaded = false;

            ItemsCache = await _usersProvider.GetUsersAsync(_session.AccessToken);
            foreach (var user in ItemsCache)
                Items.Add(user);
        }

        public async void InitializeList()
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
                    NoItemsMessage = "Нет пользователей";
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
