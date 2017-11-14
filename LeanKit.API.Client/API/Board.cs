using System.Threading.Tasks;
using LeanKit.Models;
using System.Collections.Generic;
using LeanKit.Extensions;

namespace LeanKit.API
{
    public class Board
    {
        private readonly Client _client;
        private readonly CustomField _customField;
        public Board(Client client)
        {
            _client = client;
            _customField = new CustomField(client);
        }

        public CustomField CustomField
        {
            get
            {
                return _customField;
            }
        }

		public async Task<BoardListResponse> List(BoardListRequest listRequest)
		{
            var parameters = new Dictionary<string, string>();
            if (listRequest.Offset.HasValue) {
                parameters.Add("offset", listRequest.Offset.Value.ToString());
            }
			if (listRequest.Limit.HasValue)
			{
				parameters.Add("limit", listRequest.Limit.Value.ToString());
			}
			if (listRequest.InvertSort.HasValue)
			{
				parameters.Add("invert", listRequest.InvertSort.Value.ToString());
			}
            if (!string.IsNullOrWhiteSpace(listRequest.MinimumAccess))
			{
				parameters.Add("minimumAccess", listRequest.MinimumAccess);
			}
			if (!string.IsNullOrWhiteSpace(listRequest.Search))
			{
				parameters.Add("search", listRequest.Search);
			}
			if (listRequest.Archived.HasValue)
			{
				parameters.Add("archived", listRequest.Archived.Value.ToString());
			}

            return await _client.Request<BoardListResponse>("io/board", parameters: parameters);
		}

		public async Task<BoardResponse> Get(long boardId)
		{
			var path = string.Format("io/board/{0}", boardId);
			return await _client.Request<BoardResponse>(path);
		}

		public async Task<BoardCreateResponse> Create(BoardCreateRequest request)
		{
			var body = request.ToJSON();
			return await _client.Request<BoardCreateResponse>("io/board", "post", null, body);
		}
	}
}
