using System.Threading.Tasks;
using LeanKit.Models;
using LeanKit.Extensions;
using System.Collections.Generic;

namespace LeanKit.API
{
	public class TaskCard
	{
		private readonly Client _client;
		public TaskCard(Client client)
		{
			_client = client;
		}

		public async Task<TaskCardListResponse> List(long cardId, TaskCardListRequest request = null)
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
			var path = string.Format("io/card/{0}/tasks", cardId);
            return await _client.Request<TaskCardListResponse>(path, parameters: parameters);
		}

		public async Task<TaskCardResponse> Get(long cardId, long taskId)
		{
		  var path = string.Format("io/card/{0}/tasks/{1}", cardId, taskId);
		  return await _client.Request<TaskCardResponse>(path);
		}

		public async Task<TaskCardResponse> Create(TaskCardCreateRequest request)
		{
            if ( !request.LaneType.HasValue && string.IsNullOrEmpty(request.LaneTitle))
            {
                request.LaneType = LaneType.Ready;
            }
			var path = string.Format("io/card/{0}/tasks", request.CardId);
			return await _client.Request<TaskCardResponse>(path, "post", null, request.ToJSON());
		}

	}
}
