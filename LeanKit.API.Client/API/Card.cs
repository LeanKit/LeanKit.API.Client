using System.Collections.Generic;
using System.Threading.Tasks;
using LeanKit.Extensions;
using LeanKit.Models;

namespace LeanKit.API
{
    public class Card
    {
        private readonly Client _client;
        private readonly Comment _comment;
        private readonly Attachment _attachment;
		public Card(Client client)
        {
            _client = client;
            _comment = new Comment(client);
            _attachment = new Attachment(client);
        }

		public Comment Comment
		{
			get { return _comment; }
		}

        public Attachment Attachment
        {
            get { return _attachment; }
        }

		public async Task<CardListResponse> List(CardListRequest request)
        {
            var parameters = CardListRequestToDictionary(request);
			return await _client.Request<CardListResponse>("io/card", parameters: parameters);
        }

		public async Task<CardResponse> Get(long cardId)
		{
			var path = string.Format("io/card/{0}", cardId);
			return await _client.Request<CardResponse>(path);
		}
		
        public async Task<CardCreateResponse> Create(CardCreateRequest request)
        {
			var body = request.ToJSON();
			return await _client.Request<CardCreateResponse>("io/card", "post", null, body);
        }

        public async Task<CardResponse> Update(long cardId, List<CardUpdateOperation> operations)
        {
			var body = operations.ToJSON();
			var path = string.Format("io/card/{0}", cardId);
			return await _client.Request<CardResponse>(path, "patch", null, body);
		}

		public async Task Delete(long cardId)
		{
			await _client.Request<string>(string.Format("/io/card/{0}", cardId), "DELETE");
		}

		protected Dictionary<string, string> CardListRequestToDictionary(CardListRequest request) 
        {
			var parameters = new Dictionary<string, string>();

			if (request.Offset.HasValue)
			{
				parameters.Add("offset", request.Offset.Value.ToString());
			}
			if (request.Limit.HasValue)
			{
				parameters.Add("limit", request.Limit.Value.ToString());
			}
			if (request.LaneClassType.HasValue)
			{
				var laneClasses = new List<string>();
				if (request.LaneClassType.Value.HasFlag(CardListRequest.LaneClassTypeEnum.Active))
				{
					laneClasses.Add("active");
				}
				if (request.LaneClassType.Value.HasFlag(CardListRequest.LaneClassTypeEnum.Backlog))
				{
					laneClasses.Add("backlog");
				}
				if (request.LaneClassType.Value.HasFlag(CardListRequest.LaneClassTypeEnum.Archive))
				{
					laneClasses.Add("archive");
				}
				parameters.Add("lane_class_types", string.Join(",", laneClasses));
			}
			if (request.BoardId.HasValue)
			{
				parameters.Add("board", request.BoardId.Value.ToString());
			}
			if (request.Since.HasValue)
			{
				parameters.Add("since", request.Since.Value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssK"));
			}
			if (request.Deleted.HasValue)
			{
				parameters.Add("deleted", request.Deleted.Value.ToString().ToLower());
			}
			if (request.TypeId.HasValue)
			{
				parameters.Add("type", request.TypeId.Value.ToString());
			}
			if (request.CustomIconId.HasValue)
			{
				parameters.Add("custom_icon", request.CustomIconId.Value.ToString());
			}
			if (request.ReturnFields != null && request.ReturnFields.Count > 0)
			{
				parameters.Add("only", string.Join(",", request.ReturnFields));
			}
			if (request.OmitFields != null && request.OmitFields.Count > 0)
			{
				parameters.Add("omit", string.Join(",", request.OmitFields));
			}
			if (!string.IsNullOrWhiteSpace(request.Sort))
			{
				parameters.Add("sort", request.Sort);
			}
			if (!string.IsNullOrWhiteSpace(request.Search))
			{
				parameters.Add("search", request.Search);
			}
            return parameters;
        }
    }
}
