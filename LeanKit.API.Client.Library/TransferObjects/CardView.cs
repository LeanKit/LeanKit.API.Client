//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LeanKit.API.Client.Library.TransferObjects
{
	[DebuggerDisplay("{GetType().Name,nq} [{Id}] {Title}")]
	public class CardView
	{
		public string SystemType { get; set; }
		public long Id { get; set; }
		public long LaneId { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public CardType Type { get; set; }
		public long TypeId { get; set; }
		public PriorityType Priority { get; set; }
		public string PriorityText { get { return Priority.ToString(); } }
		public string TypeName { get; set; }
		public string TypeIconPath { get; set; }
		public string TypeColorHex { get; set; }
		public int Size { get; set; }
		public bool Active { get; set; }
		public string Color { get; set; }
		public long Version { get; set; }
		public bool IsBlocked { get; set; }
		public string BlockReason { get; set; }
		public int Index { get; set; }
		public string StartDate { get; set; }
		public string DueDate { get; set; }
		public string ExternalSystemName { get; set; }
		public string ExternalSystemUrl { get; set; }
		public string ExternalCardID { get; set; }
		public string Tags { get; set; }
		public long? ClassOfServiceId { get; set; }
		public string ClassOfServiceTitle { get; set; }
		public string ClassOfServiceIconPath { get; set; }
		public string ClassOfServiceColorHex { get; set; }
		public int CountOfOldCards { get; set; }
		public string LastMove { get; set; }
		public string LastActivity { get; set; }
		public string DateArchived { get; set; }
		public string LastComment { get; set; }
		public int CommentsCount { get; set; }
		public string LastAttachment { get; set; }
		public int AttachmentsCount { get; set; }
		public long? AssignedUserId { get; set; }
		public long[] AssignedUserIds { get; set; }
		public List<AssignedUserInfo> AssignedUsers { get; set; }
		public long? DrillThroughBoardId { get; set; }
		public bool HasDrillThroughBoard { get { return DrillThroughBoardId.HasValue; } }
		public List<Comment> Comments { get; set; }
		public List<CardEvent> HistoryEvents { get; set; }
		public int? ParentCardId { get; set; }
		public int? ParentBoardId { get; set; }
		public string ExternalCardIdPrefix { get; set; }
		public string ActualFinishDate { get; set; }
		public string ActualStartDate { get; set; }
		public string AssignedUserName { get; set; }
		public string BlockStateChangeDate { get; set; }
		public long BoardId { get; set; }
		public string BoardTitle { get; set; }
		public List<CardContextView> CardContexts { get; set; }
		public string CardTypeIconColor { get; set; }
		public string CardTypeIconName { get; set; }
		public string ClassOfServiceCustomIconColor { get; set; }
		public string ClassOfServiceCustomIconName { get; set; }
		public string CreateDate { get; set; }
		public string CurrentContext { get; set; }
		public long? CurrentTaskBoardId { get; set; }
		public int? DrillThroughCompletionPercent { get; set; }
		public int? DrillThroughProgressTotal { get; set; }
		public int? DrillThroughProgressComplete { get; set; }
		public int? DrillThroughProgressSizeComplete { get; set; }
		public int? DrillThroughProgressSizeTotal { get; set; }
		public string GravatarLink { get; set; }
		public string Icon { get; set; }
		public bool IsOlderThanXDays { get; set; }
		public string LaneTitle { get; set; }
		public long? ParentTaskboardId { get; set; }
		public string SmallGravatarLink { get; set; }
		public int? TaskBoardCompletionPercent { get; set; }
		public int TaskBoardCompletedCardCount { get; set; }
		public int TaskBoardCompletedCardSize { get; set; }
		public int? TaskBoardTotalCards { get; set; }
		public int? TaskBoardTotalSize { get; set; }

		public Card ToCard()
		{
			var card = new Card();
			card.Active = Active;
			card.ClassOfServiceId = ClassOfServiceId;
			card.BlockReason = BlockReason;
			card.Description = Description;
			card.StartDate = StartDate;
			card.DueDate = DueDate;
			card.ExternalCardID = ExternalCardID;
			card.ExternalSystemName = ExternalSystemName;
			card.ExternalSystemUrl = ExternalSystemUrl;
			card.ExternalCardIdPrefix = ExternalCardIdPrefix;
			card.Id = Id;
			card.Index = Index;
			card.IsBlocked = IsBlocked;
			card.LaneId = LaneId;
			card.Priority = (int) Priority;
			card.Size = Size;
			card.Tags = Tags;
			card.Title = Title;
			card.TypeId = TypeId;
			card.Version = Version;
			card.AssignedUserIds = (AssignedUsers != null) ? (AssignedUserIds != null) ? AssignedUserIds : AssignedUsers.Select(x => x.Id.Value).ToArray() : null;
			card.Comments = Comments;
			card.HistoryEvents = HistoryEvents;
			card.LastMove = LastMove;
			card.LastActivity = LastActivity;
			card.LastComment = LastComment;
			card.DateArchived = DateArchived;
			card.ParentCardId = ParentCardId;
			card.ParentBoardId = ParentBoardId;
			card.ActualFinishDate = ActualFinishDate;
			card.ActualStartDate = ActualStartDate;
			card.AssignedUserName = AssignedUserName;
			card.BlockStateChangeDate = BlockStateChangeDate;
			card.BoardId = BoardId;
			card.BoardTitle = BoardTitle;
			card.CardContexts = CardContexts;
			card.CardTypeIconColor = CardTypeIconColor;
			card.CardTypeIconName = CardTypeIconName;
			card.ClassOfServiceCustomIconColor = ClassOfServiceCustomIconColor;
			card.ClassOfServiceCustomIconName = ClassOfServiceCustomIconName;
			card.CreateDate = CreateDate;
			card.CurrentContext = CurrentContext;
			card.CurrentTaskBoardId = CurrentTaskBoardId;
			card.DrillThroughCompletionPercent = DrillThroughCompletionPercent;
			card.DrillThroughProgressTotal = DrillThroughProgressTotal;
			card.DrillThroughProgressComplete = DrillThroughProgressComplete;
			card.DrillThroughProgressSizeComplete = DrillThroughProgressSizeComplete;
			card.DrillThroughProgressSizeTotal = DrillThroughProgressSizeTotal;
			card.GravatarLink = GravatarLink;
			card.Icon = Icon;
			card.IsOlderThanXDays = IsOlderThanXDays;
			card.LaneTitle = LaneTitle;
			card.ParentTaskboardId = ParentTaskboardId;
			card.SmallGravatarLink = SmallGravatarLink;
			card.TaskBoardCompletionPercent = TaskBoardCompletionPercent;
			card.TaskBoardCompletedCardCount = TaskBoardCompletedCardCount;
			card.TaskBoardCompletedCardSize = TaskBoardCompletedCardSize;
			card.TaskBoardTotalCards = TaskBoardTotalCards;
			card.TaskBoardTotalSize = TaskBoardTotalSize;

			return card;
		}
	}
}