namespace LeanKit.API.Legacy.Library.TransferObjects
{
	public class SearchOptions
	{
		public SearchOptions()
		{
			IncludeArchiveOnly = false;
			IncludeBacklogOnly = false;
			IncludeComments = false;
			UseFuzzySearch = false;
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
			AssignedUserIds = new long[0];
		}

		public string SearchTerm { get; set; }
		public bool ExcludeArchiveAndBacklog { get; set; }
		public bool IncludeArchiveOnly { get; set; }
		public bool IncludeBacklogOnly { get; set; }
		public bool IncludeComments { get; set; }
		public bool IncludeExternalId { get; set; }
		public bool IncludeTaskBoards { get; set; }
		public string LaneId { get; set; }
		public string AddedBefore { get; set; }
		public string AddedAfter { get; set; }
		public bool IncludeTags { get; set; }
		public long[] CardTypeIds { get; set; }
		public long[] ClassOfServiceIds { get; set; }
		public bool UseFuzzySearch { get; set; }
		public int Page { get; set; }
		public int MaxResults { get; set; }
		public string OrderBy { get; set; }
		public SortOrder SortOrder { get; set; }
		public long[] AssignedUserIds { get; set; }
		public bool SearchInBoard { get; set; }
		public bool SearchInBacklog { get; set; }
		public bool SearchInRecentArchive { get; set; }
		public bool SearchInOldArchive { get; set; }
	}

	public enum SortOrder
	{
		Ascending = 0,
		Descending = 1,
	}
}