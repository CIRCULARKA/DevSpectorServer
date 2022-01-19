using System;
using InvMan.Common.SDK.Models;

namespace InvMan.Desktop.Service
{
    public interface IApplicationEvents
    {
        event Action<Appliance> ApplianceSelected;

        void RaiseApplianceSelected(Appliance appliance);
    }
}