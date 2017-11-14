using System.Collections.Generic;

namespace LeanKit.API.Legacy.Library.TransferObjects 
{
	public class PaginationResult<T> 
	{
		public IEnumerable<T> Results { get; set; }
		public int TotalResults { get; set; }
		public int Page { get; set; }
		public int MaxResults { get; set; }
	}
}
