using System.Threading.Tasks;
using LeanKit.Models;

namespace LeanKit.API
{
    public class Account
    {
        private readonly Client _client;
        public Account(Client client)
        {
            _client = client;
        }

		public async Task<AccountResponse> Get()
		{
			return await _client.Request<AccountResponse>("io/account");
		}
	}
}
