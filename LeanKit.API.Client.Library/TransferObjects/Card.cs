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
		public virtual long Id { get; set; }

		[DomainIdentityValidation]
		public virtual long LaneId { get; set; }

		[Required(ErrorMessage = "Title may not be empty")]
		[StringLength(255, ErrorMessage = "Title may have 255 symbols only")]
		public virtual string Title { get; set; }

		[StringLength(2000, ErrorMessage = "Description may have 2000 symbols only")]
		public virtual string Description { get; set; }

		public virtual string CreatedOn { get; set; }
		public virtual string TypeName { get; set; }

		[DomainIdentityValidation]
		public virtual long TypeId { get; set; }

		public virtual int Priority { get; set; }
		public virtual int Size { get; set; }
		public virtual bool Active { get; set; }
		public virtual string Color { get; set; }
		public virtual long Version { get; set; }
		public virtual bool IsBlocked { get; set; }
		public virtual string BlockReason { get; set; }
		public virtual int Index { get; set; }
		public virtual string DueDate { get; set; }
		public virtual string UserWipOverrideComment { get; set; }
		public virtual string ExternalSystemName { get; set; }

		[StringLength(2000, ErrorMessage = "External System Url may have 2000 symbols only")]
		public virtual string ExternalSystemUrl { get; set; }

		public virtual string Tags { get; set; }

		[DomainIdentityValidation]
		public virtual long? ClassOfServiceId { get; set; }

		[StringLength(50, ErrorMessage = "Card ID may have 50 symbols only")]
		public virtual string ExternalCardID { get; set; }

		[DomainIdentityValidation]
		public virtual long[] AssignedUserIds { get; set; }

		public virtual string LastMove { get; set; }
		public virtual string LastActivity { get; set; }
		public virtual string DateArchived { get; set; }
		public virtual string LastComment { get; set; }
		public virtual int CommentsCount { get; set; }
		public List<Comment> Comments { get; set; }
		public List<CardEvent> HistoryEvents { get; set; }
		public List<CardContextView> CardContexts { get; set; }

		public CardView ToCardView()
		{
			var cardView = new CardView();
			cardView.Id = Id;
			cardView.Active = Active;
			cardView.ClassOfServiceId = ClassOfServiceId;
			cardView.BlockReason = BlockReason;
			cardView.Description = Description;
			cardView.DueDate = DueDate;
			cardView.ExternalCardID = ExternalCardID;
			cardView.ExternalSystemName = ExternalSystemName;
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

			return cardView;
		}
	}

	public class CardUpdateResult
	{
		public virtual int BoardVersion { get; set; }
		public virtual CardView CardDTO { get; set; }
	}

	public class CardAddResult
	{
		public virtual int BoardVersion { get; set; }
		public virtual Lane Lane { get; set; }
		public virtual long CardId { get; set; }
	}

	public class CardMoveResult
	{
		public virtual long BoardVersion { get; set; }
	}

	public class CardMoveBetweenBoardsResult
	{
		public virtual long BoardVersion { get; set; }
		public virtual string DestinationLaneName { get; set; }
		public virtual long DestinationLaneId { get; set; }
		public virtual long SourceBoardId { get; set; }
		public virtual long DestinationBoardId { get; set; }
	}

	public class CardsUpdateResult
	{
		public virtual int UpdatedCardsCount { get; set; }
	}

	public class CardsDeleteResult
	{
		public virtual long BoardVersion { get; set; }
	}
}