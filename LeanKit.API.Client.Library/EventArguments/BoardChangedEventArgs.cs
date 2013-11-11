//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using LeanKit.API.Client.Library.TransferObjects;

namespace LeanKit.API.Client.Library.EventArguments
{
	public class BoardChangedEventArgs : EventArgs
	{
		public BoardChangedEventArgs()
		{
			MovedCards = new List<CardMoveEvent>();
			UpdatedCards = new List<CardUpdateEvent>();
			AddedCards = new List<CardAddEvent>();
			DeletedCards = new List<CardDeletedEvent>();
			BlockedCards = new List<CardBlockedEvent>();
			UnBlockedCards = new List<CardUnBlockedEvent>();
			AssignedUsers = new List<CardUserAssignmentEvent>();
			UnAssignedUsers = new List<CardUserUnAssignmentEvent>();
			PostedComments = new List<CommentPostedEvent>();
			WipOverrides = new List<WipOverrideEvent>();
			UserWipOverrides = new List<UserWipOverrideEvent>();
			AttachmentChangedEvents = new List<AttachmentChangedEvent>();
			CardMoveToBoardEvents = new List<CardMoveToBoardEvent>();
			CardMoveFromBoardEvents = new List<CardMoveFromBoardEvent>();
			BoardEditedEvents = new List<BoardEditedEvent>();
			BoardCardTypesChangedEvents = new List<BoardCardTypesChangedEvent>();
			BoardClassOfServiceChangedEvents = new List<BoardClassOfServiceChangedEvent>();
		}

		public List<CardMoveEvent> MovedCards { get; set; }
		public List<CardUpdateEvent> UpdatedCards { get; set; }
		public List<CardAddEvent> AddedCards { get; set; }
		public bool BoardStructureChanged { get; set; }
		public Board UpdatedBoard { get; set; }
		public List<CardDeletedEvent> DeletedCards { get; set; }
		public List<CardBlockedEvent> BlockedCards { get; set; }
		public List<CardUnBlockedEvent> UnBlockedCards { get; set; }
		public List<CardUserAssignmentEvent> AssignedUsers { get; set; }
		public List<CardUserUnAssignmentEvent> UnAssignedUsers { get; set; }
		public List<CommentPostedEvent> PostedComments { get; set; }
		public List<WipOverrideEvent> WipOverrides { get; set; }
		public List<UserWipOverrideEvent> UserWipOverrides { get; set; }
		public List<AttachmentChangedEvent> AttachmentChangedEvents { get; set; }
		public List<CardMoveToBoardEvent> CardMoveToBoardEvents { get; set; }
		public List<CardMoveFromBoardEvent> CardMoveFromBoardEvents { get; set; }
		public List<BoardEditedEvent> BoardEditedEvents { get; set; }
		public List<BoardCardTypesChangedEvent> BoardCardTypesChangedEvents { get; set; }
		public List<BoardClassOfServiceChangedEvent> BoardClassOfServiceChangedEvents { get; set; }
		public bool BoardWasReloaded { get; set; }
		public long BoardVersion { get; set; }
	}

	public class UserWipOverrideEvent : EventBase
	{
		public UserWipOverrideEvent(DateTime eventDateTime, string comment, User user)
			: base(eventDateTime)
		{
			WipOverrideComment = comment;
			UserToOverrideWip = user;
		}

		public string WipOverrideComment { get; private set; }
		public User UserToOverrideWip { get; private set; }
	}

	public class WipOverrideEvent : EventBase
	{
		public WipOverrideEvent(DateTime eventDateTime, string comment, Lane lane)
			: base(eventDateTime)
		{
			WipOverrideComment = comment;
			LaneToOverrideWip = lane;
		}

		public string WipOverrideComment { get; private set; }
		public Lane LaneToOverrideWip { get; private set; }
	}

	public class CardDeletedEvent : EventBase
	{
		public CardDeletedEvent(DateTime eventDateTime, Card deletedCard) : base(eventDateTime)
		{
			DeletedCard = deletedCard;
		}

		public Card DeletedCard { get; private set; }
	}

	public class CommentPostedEvent : EventBase
	{
		public CommentPostedEvent(DateTime eventDateTime, Card card, String commentText)
			: base(eventDateTime)
		{
			CommentText = commentText;
			Card = card;
		}

		public String CommentText { get; private set; }
		public Card Card { get; private set; }
	}

	public class CardUserAssignmentEvent : EventBase
	{
		public CardUserAssignmentEvent(DateTime eventDateTime, Card affectedCard, User user)
			: base(eventDateTime)
		{
			AffectedCard = affectedCard;
			User = user;
		}

		public Card AffectedCard { get; private set; }
		public User User { get; private set; }
	}

	public class CardUserUnAssignmentEvent : EventBase
	{
		public CardUserUnAssignmentEvent(DateTime eventDateTime, Card affectedCard, User user)
			: base(eventDateTime)
		{
			AffectedCard = affectedCard;
			User = user;
		}

		public Card AffectedCard { get; private set; }
		public User User { get; private set; }
	}


	public class CardBlockedEvent : EventBase
	{
		public CardBlockedEvent(DateTime eventDateTime, Card blockedCard, string message)
			: base(eventDateTime)
		{
			BlockedCard = blockedCard;
			Message = message;
		}

		public Card BlockedCard { get; private set; }
		public String Message { get; private set; }
	}

	public class CardUnBlockedEvent : EventBase
	{
		public CardUnBlockedEvent(DateTime eventDateTime, Card unBlockedCard, string message)
			: base(eventDateTime)
		{
			UnBlockedCard = unBlockedCard;
			Message = message;
		}

		public Card UnBlockedCard { get; private set; }
		public String Message { get; private set; }
	}

	public class CardAddEvent : EventBase
	{
		public CardAddEvent(DateTime eventDateTime, Card addedCard) : base(eventDateTime)
		{
			AddedCard = addedCard;
		}

		public Card AddedCard { get; private set; }
	}

	public class CardUpdateEvent : EventBase
	{
		public CardUpdateEvent(DateTime eventDateTime, Card originalCard, Card updatedCard) : base(eventDateTime)
		{
			OriginalCard = originalCard;
			UpdatedCard = updatedCard;
		}

		public Card OriginalCard { get; private set; }
		public Card UpdatedCard { get; private set; }
	}


	public class CardMoveEvent : EventBase
	{
		public CardMoveEvent(DateTime eventDateTime, Lane fromLane, Lane toLane, Card movedCard) : base(eventDateTime)
		{
			FromLane = fromLane;
			ToLane = toLane;
			MovedCard = movedCard;
		}

		public Lane FromLane { get; private set; }
		public Lane ToLane { get; private set; }
		public Card MovedCard { get; private set; }
	}

	public class AttachmentChangedEvent : EventBase
	{
		public AttachmentChangedEvent(DateTime eventDateTime, Card attachedToCard, string fileName, string comment,
			bool isFileBeingDeleted) : base(eventDateTime)
		{
			AttachedToCard = attachedToCard;
			FileName = fileName;
			Comment = comment;
			IsFileBeingDeleted = isFileBeingDeleted;
		}

		public Card AttachedToCard { get; set; }
		public string FileName { get; set; }
		public string Comment { get; set; }
		public bool IsFileBeingDeleted { get; set; }
	}

	public class CardMoveToBoardEvent : EventBase
	{
		public CardMoveToBoardEvent(DateTime eventDateTime, long cardId, long? toLaneId) : base(eventDateTime)
		{
			CardId = cardId;
			ToLaneId = toLaneId;
		}

		public long CardId { get; set; }
		public long? ToLaneId { get; set; }
	}

	public class CardMoveFromBoardEvent : EventBase
	{
		public CardMoveFromBoardEvent(DateTime eventDateTime, long cardId, long? fromLaneId) : base(eventDateTime)
		{
			CardId = cardId;
			FromLaneId = fromLaneId;
		}

		public long CardId { get; set; }
		public long? FromLaneId { get; set; }
	}

	public class BoardEditedEvent : EventBase
	{
		public BoardEditedEvent(DateTime eventDateTime, string comment) : base(eventDateTime)
		{
			Comment = comment;
		}

		public string Comment { get; set; }
	}

	public class BoardCardTypesChangedEvent : EventBase
	{
		public BoardCardTypesChangedEvent(DateTime eventDateTime) : base(eventDateTime)
		{
		}
	}

	public class BoardClassOfServiceChangedEvent : EventBase
	{
		public BoardClassOfServiceChangedEvent(DateTime eventDateTime) : base(eventDateTime)
		{
		}
	}

	public abstract class EventBase
	{
		protected EventBase(DateTime eventDateTime)
		{
			EventDateTime = eventDateTime;
		}

		public virtual DateTime EventDateTime { get; protected set; }
	}
}