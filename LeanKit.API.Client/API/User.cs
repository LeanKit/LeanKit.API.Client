using System.Threading.Tasks;
using LeanKit.Models;
using System.Collections.Generic;

namespace LeanKit.API
{
	public class User
	{
		private readonly Client _client;
        private readonly UserBoards _boards;
		public User(Client client)
		{
			_client = client;
            _boards = new UserBoards(client);
		}

        public UserBoards Boards
        {
            get { return _boards; }
        }

        public async Task<UserListResponse> List(UserListRequest request = null)
		{
			var parameters = new Dictionary<string, string>();
            if (request != null)
            {
                if (request.Offset.HasValue)
                {
                    parameters.Add("offset", request.Offset.Value.ToString());
                }
                if (request.Limit.HasValue)
                {
                    parameters.Add("limit", request.Limit.Value.ToString());
                }
            }
			return await _client.Request<UserListResponse>("io/user", parameters: parameters);
		}

		public async Task<UserResponse> Get(long userId)
		{
			return await _client.Request<UserResponse>(string.Format("io/user/{0}", userId));
		}

		public async Task<UserResponse> Me()
		{
			return await _client.Request<UserResponse>("io/user/me");
		}
	}
}
