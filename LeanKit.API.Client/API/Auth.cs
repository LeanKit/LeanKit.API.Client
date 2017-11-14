using System;
using System.Threading.Tasks;
using LeanKit.Extensions;
using LeanKit.Models;

namespace LeanKit.API
{
    public class Auth
    {
        private readonly Client _client;
        private readonly Token _token;
        public Auth(Client client)
        {
            _client = client;
            _token = new Token(client);
        }
        public Token Token
        {
            get { return _token; }
        }
    }

    public class Token
    {
        private readonly Client _client;
        public Token(Client client)
        {
            _client = client;
        }

        public async Task<TokenList> List()
        {
            return await _client.Request<TokenList>("io/auth/token");
        }

        public async Task<TokenCreateResponse> Create(string description)
        {
            if (string.IsNullOrWhiteSpace(description)) 
            {
                throw new ArgumentException("Description is required");
            }
            var body = new { description };
            return await _client.Request<TokenCreateResponse>("/io/auth/token", "POST", body: body.ToJSON());
        }

        public async Task Revoke(long id)
        {
            await _client.Request<string>(string.Format("/io/auth/token/{0}", id), "DELETE");
        }
    }
}
