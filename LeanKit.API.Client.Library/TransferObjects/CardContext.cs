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
		public long TaskBoardId { get; set; }
		public long Id { get; set; }
		public string Name { get; set; }
		public int ProgressPercentage { get; set; }
		public int TotalCards { get; set; }
	}
}