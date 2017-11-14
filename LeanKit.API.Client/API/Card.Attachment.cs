using System.Threading.Tasks;
using LeanKit.Models;
using System.Collections.Generic;

namespace LeanKit.API
{
	public class Attachment
	{
		private readonly Client _client;
		public Attachment(Client client)
		{
			_client = client;
		}

		public async Task<CardAttachmentListResponse> List(long cardId)
		{
			var path = string.Format("io/card/{0}/attachment", cardId);
			return await _client.Request<CardAttachmentListResponse>(path);
		}

		public async Task<AttachmentResponse> Create(long cardId, string name, string description, byte[] fileBytes)
		{
			var path = string.Format("io/card/{0}/attachment", cardId);
			var metaContent = new Dictionary<string, string>
			{
				{ "Id", "0" },
				{ "Description", description },
				{ "FileName", name }
			};
			return await _client.PostFile<AttachmentResponse>(path, metaContent, name, fileBytes);
		}

		public async Task<AttachmentFileResponse> Download(long cardId, long attachmentId)
		{
			var path = string.Format("io/card/{0}/attachment/{1}/content", cardId, attachmentId);
			return await _client.GetFile(path);
		}

		public async Task Delete(long cardId, long attachmentId)
		{
			await _client.Request<string>(string.Format("io/card/{0}/attachment/{1}", cardId, attachmentId), "DELETE");
		}
	}
}
