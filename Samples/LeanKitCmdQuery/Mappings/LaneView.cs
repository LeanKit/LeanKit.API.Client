//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

namespace LeanKitCmdQuery.Mappings
{
	public class LaneView
	{
		public long? Id { get; set; }
		public int Index { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string ClassType { get; set; }
		public string ActivityName { get; set; }
		public long ParentLaneId { get; set; }
		public string Orientation { get; set; }
		public long? TaskBoardId { get; set; }
		public int CardLimit { get; set; }
		public string LaneState { get; set; }
		public int Cards { get; set; }
		public string ChildLaneIds { get; set; }
		public string SiblingLaneIds { get; set; }
	}
}