//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;

namespace LeanKit.API.Client.Library.TransferObjects 
{
	public class PaginationResult<T> 
	{
		public IEnumerable<T> Results { get; set; }
		public int TotalResults { get; set; }

		public int Page { get; set; }
		public int MaxResults { get; set; }
	}
}
