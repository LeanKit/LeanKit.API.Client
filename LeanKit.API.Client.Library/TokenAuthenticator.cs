using System;
using System.Linq;
using RestSharp;

namespace LeanKit.API.Client.Library
{
	public class TokenAuthenticator : IAuthenticator
	{
		private readonly string _token;

		public TokenAuthenticator(string token)
		{
			_token = token;
		}

		public void Authenticate(IRestClient client, IRestRequest request)
		{
			if (request.Parameters.Any(p => p.Name.Equals("Authorization", StringComparison.OrdinalIgnoreCase))) return;
			var authHeader = string.Format("Token {0}", _token);
			request.AddParameter("Authorization", authHeader, ParameterType.HttpHeader);
		}
	}
}
