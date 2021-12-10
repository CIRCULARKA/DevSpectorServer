using System;
using System.Collections.Generic;
using System.Text;
using InvMan.Common.SDK;
using InvMan.Common.SDK.Models;

namespace InvMan.Desktop.UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IRawDataProvider _jsonProvider;

        private readonly IDevicesProvider _devicesProvider;

        private MainWindowViewModel() { }

        public MainWindowViewModel(IRawDataProvider rawDataProvider, IDevicesProvider devicesProvider)
        {
            _jsonProvider = rawDataProvider;
            _devicesProvider = devicesProvider;
        }
    }
}
