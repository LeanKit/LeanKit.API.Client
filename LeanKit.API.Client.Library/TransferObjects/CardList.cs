//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

namespace LeanKit.API.Client.Library.TransferObjects
{
	public class CardList
	{
		public virtual long Id { get; set; }
		public virtual long LaneId { get; set; }
		public virtual string Title { get; set; }
		public virtual string Description { get; set; }
		public virtual string CreatedOn { get; set; }
		public virtual string TypeName { get; set; }
		public virtual long TypeId { get; set; }
		public virtual PriorityType Priority { get; set; }
		public virtual int Size { get; set; }
		public virtual int Index { get; set; }
		public virtual string DueDate { get; set; }
		public virtual string ExternalSystemName { get; set; }
		public virtual string ExternalSystemUrl { get; set; }
		public virtual string Tags { get; set; }
		public virtual long? ClassOfServiceId { get; set; }
		public virtual string ExternalCardID { get; set; }
		public virtual long? DrillThroughBoardId { get; set; }
		public virtual long? ParentCardId { get; set; }
	}
}