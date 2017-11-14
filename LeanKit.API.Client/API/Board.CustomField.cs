using System.Threading.Tasks;
using LeanKit.Models;
using System.Collections.Generic;
using LeanKit.Extensions;

namespace LeanKit.API
{
    public class CustomField
    {
        private readonly Client _client;
        public CustomField(Client client)
        {
            _client = client;
        }

		public async Task<CustomFieldListResponse> List(long boardId)
		{
			var path = string.Format("io/board/{0}/customfield", boardId);
			return await _client.Request<CustomFieldListResponse>(path);
		}

		public async Task<CustomFieldListResponse> Update(long boardId, List<BoardCustomFieldUpdateOperation> operations)
		{
            var body = operations.ToJSON();
			var path = string.Format("io/board/{0}/customfield", boardId);
			return await _client.Request<CustomFieldListResponse>(path, "patch", null, body);
		}
	}
}
