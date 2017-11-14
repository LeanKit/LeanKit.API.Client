using System;
using System.Threading.Tasks;
using LeanKit.Extensions;
using LeanKit.Models;

namespace LeanKit.API
{
    public class Template
    {
		private readonly Client _client;
		public Template(Client client)
		{
			_client = client;
		}

		public async Task<TemplateListResponse> List()
		{
			return await _client.Request<TemplateListResponse>("io/template");
		}

		public async Task<TemplateCreateResponse> Create(TemplateCreateRequest request)
		{
			var body = request.ToJSON();
            return await _client.Request<TemplateCreateResponse>("io/template", "post", null, body);
		}

		public async Task Delete(long templateId)
		{
			await _client.Request<string>(string.Format("/io/template/{0}", templateId), "DELETE");
		}
	}
}
