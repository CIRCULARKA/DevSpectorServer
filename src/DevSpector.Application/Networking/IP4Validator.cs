using System;
using System.Text.RegularExpressions;

namespace DevSpector.Application.Networking
{
    public class IP4Validator : IIPValidator
    {
        private const string _Ip4Pattern =
            @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";

		public bool Matches(string value)
        {
			try { return Regex.IsMatch(value, _Ip4Pattern); }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException(
                    "Could not validate IP address - value can not be null"
                );
            }
        }
    }
}
