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
    public class UsersListViewModel : ViewModelBase, IUsersListViewModel
    {
        private readonly IApplicationEvents _appEvents;

        // private readonly IUsersProvider _usersProvider;

        private readonly IUserSession _session;

        // private User _selectedUser;

        // private IEnumerable<User> _usersCache;

        private string _noUsersMessage;

        private bool _areThereUsers;

        private bool _areUsersLoaded;

        public UsersListViewModel(
            // IUsersProvider usersProvider,
            IApplicationEvents appEvents,
            IUserSession session
        )
        {
            _appEvents = appEvents;
            _session = session;
            // _usersProvider = usersProvider;
            _areUsersLoaded = false;
            // _usersCache = new List<User>();

            // Users = new ObservableCollection<User>();
        }

        // public ObservableCollection<User> Users { get; set; }

        // public IEnumerable<User> CachedUsers => _usersCache;

        // public User SelectedUser
        // {
        //     get => _selectedUser;
        //     set
        //     {
        //         this.RaiseAndSetIfChanged(ref _selectedUser, value);

        //         _appEvents.RaiseUserSelected(_selectedUser);
        //     }
        // }

        public bool AreUsersLoaded
        {
            get => _areUsersLoaded;
            set => this.RaiseAndSetIfChanged(ref _areUsersLoaded, value);
        }

        public bool AreThereUsers
        {
            get => _areThereUsers;
            set { this.RaiseAndSetIfChanged(ref _areThereUsers, value); }
        }

        public string NoUsersMessage
        {
            get => _noUsersMessage;
            set { this.RaiseAndSetIfChanged(ref _noUsersMessage, value); }
        }

        // public void LoadUsers(IEnumerable<User> devices)
        // {
        //     Users.Clear();

        //     foreach (var device in devices)
        //         Users.Add(device);

        //     if (Users.Count == 0) {
        //         AreThereUsers = false;
        //         NoUsersMessage = "Устройства не найдены";
        //     }
        //     else AreThereUsers = true;

        // }

        // private async Task LoadUsers()
        // {
        //     AreUsersLoaded = false;

        //     _devicesCache = await _usersProvider.GetDevicesAsync(_session.AccessToken);
        //     foreach (var device in _devicesCache)
        //         Users.Add(device);
        // }

        // public async void InitializeList()
        // {
        //     try
        //     {
        //         await LoadUsers();

        //         if (Users.Count > 0) {
        //             AreThereUsers = true;
        //             SelectedUser = Users[0];
        //         }
        //         else {
        //             AreThereUsers = false;
        //             NoUsersMessage = "Нет устройств";
        //         }
        //     }
        //     catch (ArgumentException)
        //     {
        //         AreThereUsers = false;
        //         NoUsersMessage = "Ошибка доступа";
        //     }
        //     catch
        //     {
        //         AreThereUsers = false;
        //         NoUsersMessage = "Что-то пошло не так";
        //     }
        //     finally { AreUsersLoaded = true; }
        // }
    }
}
