using System;

namespace InvMan.Common.SDK.Factories
{
    public interface IDevicesProviderFactory
    {
        IDevicesProvider CreateDefaultDevicesProvider(Uri hostAddress);
    }
}
