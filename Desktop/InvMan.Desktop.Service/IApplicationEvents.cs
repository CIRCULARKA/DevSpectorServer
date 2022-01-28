using System;
using System.Collections.Generic;
using InvMan.Common.SDK.Models;

namespace InvMan.Desktop.Service
{
    public interface IApplicationEvents
    {
        event Action<Appliance> ApplianceSelected;

        void RaiseApplianceSelected(Appliance appliance);

        event Action<IEnumerable<Appliance>> SearchExecuted;

        void RaiseSearchExecuted(IEnumerable<Appliance> filtered);

        event Action UserAuthorized;

        void RaiseUserAuthorized();
    }
}
