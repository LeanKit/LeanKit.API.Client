using System;
using System.Collections.Generic;

namespace LeanKit.Models
{
    public class TokenList 
    {
        public TokenList() 
        {
            Tokens = new List<TokenListResponse>();
        }
        public List<TokenListResponse> Tokens { get; set; }   
    }

    public class TokenListResponse
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class TokenCreateResponse
    {
		public long Id { get; set; }
		public string Token { get; set; }
		public string Description { get; set; }
		public DateTime CreatedOn { get; set; }
	}
}
