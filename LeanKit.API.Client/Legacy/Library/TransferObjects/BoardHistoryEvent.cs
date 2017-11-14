using System;

namespace LeanKit.API.Legacy.Library.TransferObjects
{
	public class BoardHistoryEvent
	{
		public long CardId { get; set; }
		public string EventType { get; set; }
		public DateTime EventDateTime { get; set; }
		public string Message { get; set; }
		public long ToLaneId { get; set; }
		public long? FromLaneId { get; set; }
		public bool RequiresBoardRefresh { get; set; }

		public bool IsBlocked { get; set; }
		public string BlockedComment { get; set; }
		public long UserId { get; set; }
		public long AssignedUserId { get; set; }
		public bool IsUnassigning { get; set; }
		public string CommentText { get; set; }
		public string WipOverrideComment { get; set; }
		public long WipOverrideLane { get; set; }
		public long WipOverrideUser { get; set; }
		public string FileName { get; set; }
		public bool IsFileBeingDeleted { get; set; }
	}
}