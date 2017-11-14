using System.Threading.Tasks;
using LeanKit.Models;
using LeanKit.Extensions;

namespace LeanKit.API
{
	public class Comment
	{
        private readonly Client _client;
        public Comment(Client client)
        {
            _client = client;
        }

		public async Task<CardCommentListResponse> List(long cardId)
		{
			var path = string.Format("io/card/{0}/comment", cardId);
			return await _client.Request<CardCommentListResponse>(path);
		}

		public async Task<CardCommentResponse> Create(long cardId, string text)
		{
            var request = new { text };
			var body = request.ToJSON();
            return await _client.Request<CardCommentResponse>(string.Format("io/card/{0}/comment", cardId), "post", null, body);
		}

		public async Task<CardCommentResponse> Update(long cardId, long commentId, string text)
		{
			var request = new { text };
			var body = request.ToJSON();
            return await _client.Request<CardCommentResponse>(string.Format("io/card/{0}/comment/{1}", cardId, commentId), "put", null, body);
		}

		public async Task Delete(long cardId, long commentId)
		{
			await _client.Request<string>(string.Format("io/card/{0}/comment/{1}", cardId, commentId), "DELETE");
		}
	}
}
