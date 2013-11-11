//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

namespace LeanKit.API.Client.Library.TransferObjects
{
	public class SearchOptions
	{
		public SearchOptions()
		{
			IncludeArchiveOnly = false;
			IncludeBacklogOnly = false;
			IncludeComments = false;
			IncludeDescription = false;
			IncludeExternalId = false;
			IncludeTags = false;
			AddedAfter = null;
			AddedBefore = null;
			CardTypeIds = new long[0];
			ClassOfServiceIds = new long[0];
			Page = 1;
			MaxResults = 20;
			OrderBy = "CreatedOn";
			SortOrder = SortOrder.Ascending;
		}

		public string SearchTerm { get; set; }
		public bool ExcludeArchiveAndBacklog { get; set; }
		public bool IncludeArchiveOnly { get; set; }
		public bool IncludeBacklogOnly { get; set; }
		public bool IncludeDescription { get; set; }
		public bool IncludeComments { get; set; }
		public bool IncludeExternalId { get; set; }
		public string LaneId { get; set; }
		public string AddedBefore { get; set; }
		public string AddedAfter { get; set; }
		public bool IncludeTags { get; set; }
		public long[] CardTypeIds { get; set; }
		public long[] ClassOfServiceIds { get; set; }
		public int Page { get; set; }
		public int MaxResults { get; set; }
		public string OrderBy { get; set; }
		public SortOrder SortOrder { get; set; }
	}

	public enum SortOrder
	{
		Ascending = 0,
		Descending = 1,
	}
}