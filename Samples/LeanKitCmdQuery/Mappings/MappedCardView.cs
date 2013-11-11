//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

namespace LeanKitCmdQuery.Mappings
{
	public class MappedCardView
	{
		public long Id { get; set; }
		public long LaneId { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string Type { get; set; }
		public string Priority { get; set; }
		public int Size { get; set; }
		public string Color { get; set; }
		public long Version { get; set; }
		public string AssignedUsers { get; set; }
		public bool IsBlocked { get; set; }
		public string BlockReason { get; set; }
		public int Index { get; set; }
		public string DueDate { get; set; }
		public string ExternalSystemName { get; set; }
		public string ExternalSystemUrl { get; set; }
		public string ExternalCardID { get; set; }
		public string Tags { get; set; }
		public string ClassOfService { get; set; }
		public string LastMove { get; set; }
		public string LastActivity { get; set; }
		public string DateArchived { get; set; }
		public string LastComment { get; set; }
		public int CommentsCount { get; set; }
	}
}