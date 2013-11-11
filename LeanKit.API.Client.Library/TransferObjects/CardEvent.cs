//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;

namespace LeanKit.API.Client.Library.TransferObjects
{
	public class CardEvent
	{
		private string _type;
		public long CardId { get; set; }
		public long? ToLaneId { get; set; }
		public string ToLaneTitle { get; set; }

		public string Type
		{
			get { return _type; }
			set { _type = value.Replace("DTO", ""); }
		}

		public string UserName { get; set; }
		public string UserFullName { get; set; }
		public string DateTime { get; set; }
		public string TimeDifference { get; set; }
		public string CommentText { get; set; }
		public long? FromLaneId { get; set; }
		public string FromLaneTitle { get; set; }
		public string Comment { get; set; }
		public bool IsBlocked { get; set; }
		public IList<FieldChange> Changes { get; set; }
		public long? AssignedUserId { get; set; }
		public string AssignedUserFullName { get; set; }
		public string AssignedUserEmailAddres { get; set; }
		public bool IsUnassigning { get; set; }
		public long? UserToOverrideWipId { get; set; }
		public string UserToOverrideWipName { get; set; }
		public string WipOverrideComment { get; set; }
		public string UserToOverrideWipEmail { get; set; }
		public long? LaneToOverrideWipId { get; set; }
		public string LaneToOverrideWipTitle { get; set; }
		public bool IsDelete { get; set; }
		public string FileName { get; set; }

		public class FieldChange
		{
			public string FieldName { get; set; }
			public string OldValue { get; set; }
			public string NewValue { get; set; }
			public string OldDueDate { get; set; }
			public string NewDueDate { get; set; }
		}
	}
}