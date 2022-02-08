using System;
using System.Collections.Generic;
using InvMan.Common.SDK.Models;

namespace InvMan.Desktop.Service
{
    public class ApplicationEvents : IApplicationEvents
    {
        public ApplicationEvents() { }

        public event Action<Appliance> ApplianceSelected;

        public void RaiseApplianceSelected(Appliance appliance) =>
            ApplianceSelected?.Invoke(appliance);

        public event Action<IEnumerable<Appliance>> SearchExecuted;

        public void RaiseSearchExecuted(IEnumerable<Appliance> filtered) =>
            SearchExecuted?.Invoke(filtered);

        public event Action<User> UserAuthorized;

        public void RaiseUserAuthorized(User user) =>
            UserAuthorized?.Invoke(user);

        public event Action AuthorizationCompleted;

        public void RaiseAuthorizationCompleted() =>
            AuthorizationCompleted?.Invoke();

        public event Action<User> UserSelected;

        public void RaiseUserSelected(User user) =>
            UserSelected?.Invoke(user);
    }
}
