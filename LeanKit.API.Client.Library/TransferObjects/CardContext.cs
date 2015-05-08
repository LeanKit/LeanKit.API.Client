//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

namespace LeanKit.API.Client.Library.TransferObjects
{
	public class CardContext
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public bool IsDefault { get; set; }
	}

	public class CardContextView
	{
		public long Id { get; set; }
		public long TaskBoardId { get; set; }
		public string Name { get; set; }
		public int TaskBoardCompletionPercent { get; set; }
		public int TaskBoardCompletedCardCount { get; set; }
		public int TaskBoardCompletedCardSize { get; set; }
		public int TaskBoardTotalCards { get; set; }
		public int TaskBoardTotalSize { get; set; }
		public int ProgressPercentage { get; set; }
		public int TotalCards { get; set; }
		public int TotalSize { get; set; }
		public int CompletedCardCount { get; set; }
		public int CompletedCardSize { get; set; }
	}
}