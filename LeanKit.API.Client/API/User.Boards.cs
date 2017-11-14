using System.Threading.Tasks;
using LeanKit.Models;

namespace LeanKit.API
{
	public class UserBoards
	{
		private readonly Client _client;
		public UserBoards(Client client)
		{
			_client = client;
		}

		public async Task<UserRecentBoardsResponse> Recent()
		{
			return await _client.Request<UserRecentBoardsResponse>("io/user/me/board/recent");
		}
	}

}
