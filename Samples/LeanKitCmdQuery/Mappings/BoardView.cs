//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

namespace LeanKitCmdQuery.Mappings
{
	public class BoardLiteView
	{
		public long Id { get; set; }
		public string Title { get; set; }
	}

	public class BoardView
	{
		public long Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public long Version { get; set; }
		public long? BacklogTopLevelLaneId { get; set; }
		public long? ArchiveTopLevelLaneId { get; set; }
		public int Lanes { get; set; }
		public string ClassesOfService { get; set; }
		public string CardTypes { get; set; }
		public string TopLevelLaneIds { get; set; }
	}
}