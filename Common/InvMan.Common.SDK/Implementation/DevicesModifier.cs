using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace InvMan.Common.SDK
{
    public class DevicesModifier
    {
        private readonly HttpClient _client;

        public DevicesModifier()
        {
            _client = new HttpClient();
        }

		public async Task CreateDevice(string networkName, string inventoryNumber, string type)
		{
            var requestUrlBuilder = new UriBuilder();
            requestUrlBuilder.Scheme = "http";
            requestUrlBuilder.Port = 5000;
            requestUrlBuilder.Host = "localhost";
            requestUrlBuilder.Query =
                $"networkName={networkName}&inventoryNumber={inventoryNumber}&type={type}";

            Console.WriteLine("DEBUG -> " + requestUrlBuilder.Uri);

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                requestUrlBuilder.Uri
            );

            await _client.SendAsync(request);
		}
    }
}
