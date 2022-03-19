using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using DevSpector.Tests.Database;
using DevSpector.Application.Devices;
using DevSpector.Domain;
using DevSpector.Domain.Models;
using DevSpector.Database;
using DevSpector.Application.Networking;
using DevSpector.SDK.Models;

namespace DevSpector.Tests.Application.Devices
{
    public class DevicesEditorTests : DatabaseTestBase
    {
        private readonly IDevicesEditor _editor;

        public DevicesEditorTests()
        {
            var ipValidator = new IPValidator();

            _editor = new DevicesEditor(
                base._repo,
                new DevicesProvider(base._repo, ipValidator),
                ipValidator,
                new IPAddressProvider(base._repo, ipValidator, new IP4RangeGenerator(ipValidator))
            );
        }
    }
}
