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
		public virtual string SystemType { get; set; }
		public virtual long Id { get; set; }
		public virtual long LaneId { get; set; }
		public virtual string Title { get; set; }
		public virtual string Description { get; set; }
		public virtual CardType Type { get; set; }
		public virtual long TypeId { get; set; }
		public virtual PriorityType Priority { get; set; }

		public virtual string PriorityText
		{
			get { return Priority.ToString(); }
		}

		public virtual string TypeName { get; set; }
		public virtual string TypeIconPath { get; set; }
		public virtual string TypeColorHex { get; set; }
		public virtual int Size { get; set; }
		public virtual bool Active { get; set; }
		public virtual string Color { get; set; }
		public virtual long Version { get; set; }
		public virtual bool IsBlocked { get; set; }
		public virtual string BlockReason { get; set; }
		public virtual int Index { get; set; }
		public virtual string StartDate { get; set; }
		public virtual string DueDate { get; set; }
		public virtual string ExternalSystemName { get; set; }
		public virtual string ExternalSystemUrl { get; set; }
		public virtual string ExternalCardID { get; set; }
		public virtual string Tags { get; set; }
		public virtual long? ClassOfServiceId { get; set; }
		public virtual string ClassOfServiceTitle { get; set; }
		public virtual string ClassOfServiceIconPath { get; set; }
		public virtual string ClassOfServiceColorHex { get; set; }
		public virtual int CountOfOldCards { get; set; }
		public virtual string LastMove { get; set; }
		public virtual string LastActivity { get; set; }
		public virtual string DateArchived { get; set; }
		public virtual string LastComment { get; set; }
		public virtual int CommentsCount { get; set; }
		public string LastAttachment { get; set; }
		public int AttachmentsCount { get; set; }

		public virtual long? AssignedUserId { get; set; }
		public long[] AssignedUserIds { get; set; }
		public virtual List<AssignedUserInfo> AssignedUsers { get; set; }

		public virtual long? DrillThroughBoardId { get; set; }
		public virtual bool HasDrillThroughBoard { get { return DrillThroughBoardId.HasValue; } }
		
		public List<Comment> Comments { get; set; }
		public List<CardEvent> HistoryEvents { get; set; }

		public int? ParentCardId { get; set; }
		public int? ParentBoardId { get; set; }
		public string ExternalCardIdPrefix { get; set; }

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

			return card;
		}
	}
}