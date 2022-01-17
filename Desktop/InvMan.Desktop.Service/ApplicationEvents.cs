using System;
using InvMan.Common.SDK.Models;

namespace InvMan.Desktop.Service
{
    public class ApplicationEvents : IApplicationEvents
    {
        public ApplicationEvents() { }

        public event Func<Appliance> ApplianceSelected;
    }
}
