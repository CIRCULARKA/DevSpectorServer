using System;
using System.Text.RegularExpressions;
using DevSpector.Application.Enumerations;

namespace DevSpector.Application.Networking
{
    public class IPValidator : IIPValidator
    {
        private const string _Ip4Pattern =
            @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";

		public bool Matches(string value, IPProtocol protocolVersion)
        {
            if (protocolVersion != IPProtocol.Version4)
                throw new InvalidOperationException("Can't validate IP address - only IPv4 supported");

            return MatchesIP4(value);
        }

        private bool MatchesIP4(string value)
        {
			try { return Regex.IsMatch(value, _Ip4Pattern); }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException(
                    "Could not validate IPv4 address - value can not be null"
                );
            }
        }
    }
}
