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

namespace DevSpector.Tests.Application.Networking
{
    public class IPAddressProviderTests
    {
        private readonly IIPAddressProvider _provider;

        private readonly TestDbContext _context;

        public IPAddressProviderTests()
        {
            _context = new TestDbContext();
            var repo = new Repository(_context);
            var ipValidator = new IPValidator();

            _provider = new IPAddressProvider(
                repo,
                ipValidator,
                new IP4RangeGenerator(ipValidator)
            );
        }
    }
}
