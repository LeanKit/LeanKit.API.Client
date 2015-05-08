//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LeanKit.API.Client.Library.Validation;

namespace LeanKit.API.Client.Library.TransferObjects
{
	public enum PriorityType
	{
		Low,
		Normal,
		High,
		Critical
	}

	public class Card
	{
		public long Id { get; set; }

		[DomainIdentityValidation]
		public long LaneId { get; set; }

		[Required(ErrorMessage = "Title may not be empty")]
		[StringLength(255, ErrorMessage = "Title may have 255 symbols only")]
		public string Title { get; set; }

		[StringLength(20000, ErrorMessage = "Description may not exceed 20000 characters")]
		public string Description { get; set; }

		public string CreatedOn { get; set; }
		public string TypeName { get; set; }

		[DomainIdentityValidation]
		public long TypeId { get; set; }

		public int Priority { get; set; }
		public int Size { get; set; }
		public bool Active { get; set; }
		public string Color { get; set; }
		public long Version { get; set; }
		public bool IsBlocked { get; set; }
		public string BlockReason { get; set; }
		public int Index { get; set; }
		public string StartDate { get; set; }
		public string DueDate { get; set; }
		public string UserWipOverrideComment { get; set; }
		public string ExternalSystemName { get; set; }

		[StringLength(2000, ErrorMessage = "External System Url may have 2000 symbols only")]
		public string ExternalSystemUrl { get; set; }

		public string Tags { get; set; }

		[DomainIdentityValidation]
		public long? ClassOfServiceId { get; set; }

		[StringLength(50, ErrorMessage = "Card ID may have 50 symbols only")]
		public string ExternalCardID { get; set; }
		public string ExternalCardIdPrefix { get; set; }

		[DomainIdentityValidation]
		public long[] AssignedUserIds { get; set; }

		public string LastMove { get; set; }
		public string LastActivity { get; set; }
		public string DateArchived { get; set; }
		public string LastComment { get; set; }
		public int CommentsCount { get; set; }
		public List<Comment> Comments { get; set; }
		public List<CardEvent> HistoryEvents { get; set; }
		public List<CardContextView> CardContexts { get; set; }

		public int? ParentCardId { get; set; }
		public int? ParentBoardId { get; set; }
		public string ActualFinishDate { get; set; }
		public string ActualStartDate { get; set; }
		public string AssignedUserName { get; set; }
		public string BlockStateChangeDate { get; set; }
		public long BoardId { get; set; }
		public string BoardTitle { get; set; }
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

		public CardView ToCardView()
		{
			var cardView = new CardView();
			cardView.Id = Id;
			cardView.Active = Active;
			cardView.ClassOfServiceId = ClassOfServiceId;
			cardView.BlockReason = BlockReason;
			cardView.Description = Description;
			cardView.StartDate = StartDate;
			cardView.DueDate = DueDate;
			cardView.ExternalCardID = ExternalCardID;
			cardView.ExternalSystemName = ExternalSystemName;
			cardView.ExternalCardIdPrefix = ExternalCardIdPrefix;
			cardView.Index = Index;
			cardView.IsBlocked = IsBlocked;
			cardView.LaneId = LaneId;
			cardView.Priority = (PriorityType) Priority;
			cardView.Size = Size;
			cardView.Tags = Tags;
			cardView.Title = Title;
			cardView.TypeId = TypeId;
			cardView.Version = Version;
			//cardView.AssignedUsers = this.AssignedUserIds
			cardView.Comments = Comments;
			cardView.HistoryEvents = HistoryEvents;
			cardView.LastMove = LastMove;
			cardView.LastActivity = LastActivity;
			cardView.LastComment = LastComment;
			cardView.DateArchived = DateArchived;
			cardView.ParentBoardId = ParentBoardId;
			cardView.ParentCardId = ParentCardId;
			cardView.ActualFinishDate = ActualFinishDate;
			cardView.ActualStartDate = ActualStartDate;
			cardView.AssignedUserName = AssignedUserName;
			cardView.BlockStateChangeDate = BlockStateChangeDate;
			cardView.BoardId = BoardId;
			cardView.BoardTitle = BoardTitle;
			cardView.CardContexts = CardContexts;
			cardView.CardTypeIconColor = CardTypeIconColor;
			cardView.CardTypeIconName = CardTypeIconName;
			cardView.ClassOfServiceCustomIconColor = ClassOfServiceCustomIconColor;
			cardView.ClassOfServiceCustomIconName = ClassOfServiceCustomIconName;
			cardView.CreateDate = CreateDate;
			cardView.CurrentContext = CurrentContext;
			cardView.CurrentTaskBoardId = CurrentTaskBoardId;
			cardView.DrillThroughCompletionPercent = DrillThroughCompletionPercent;
			cardView.DrillThroughProgressTotal = DrillThroughProgressTotal;
			cardView.DrillThroughProgressComplete = DrillThroughProgressComplete;
			cardView.DrillThroughProgressSizeComplete = DrillThroughProgressSizeComplete;
			cardView.DrillThroughProgressSizeTotal = DrillThroughProgressSizeTotal;
			cardView.GravatarLink = GravatarLink;
			cardView.Icon = Icon;
			cardView.IsOlderThanXDays = IsOlderThanXDays;
			cardView.LaneTitle = LaneTitle;
			cardView.ParentTaskboardId = ParentTaskboardId;
			cardView.SmallGravatarLink = SmallGravatarLink;
			cardView.TaskBoardCompletionPercent = TaskBoardCompletionPercent;
			cardView.TaskBoardCompletedCardCount = TaskBoardCompletedCardCount;
			cardView.TaskBoardCompletedCardSize = TaskBoardCompletedCardSize;
			cardView.TaskBoardTotalCards = TaskBoardTotalCards;
			cardView.TaskBoardTotalSize = TaskBoardTotalSize;

			return cardView;
		}
	}

	public class CardUpdateResult
	{
		public int BoardVersion { get; set; }
		public CardView CardDTO { get; set; }
	}

	public class CardAddResult
	{
		public int BoardVersion { get; set; }
		public Lane Lane { get; set; }
		public long CardId { get; set; }
	}

	public class CardMoveResult
	{
		public long BoardVersion { get; set; }
	}

	public class CardMoveBetweenBoardsResult
	{
		public long BoardVersion { get; set; }
		public string DestinationLaneName { get; set; }
		public long DestinationLaneId { get; set; }
		public long SourceBoardId { get; set; }
		public long DestinationBoardId { get; set; }
	}

	public class CardsUpdateResult
	{
		public int UpdatedCardsCount { get; set; }
	}

	public class CardsDeleteResult
	{
		public long BoardVersion { get; set; }
	}

    public class CardDelegationResult
    {
        public long ParentBoardId { get; set; }
        public long DelegationCardId { get; set; }
        public long DestinationLaneId { get; set; }
        public string DestinationLaneName { get; set; }
    }

}