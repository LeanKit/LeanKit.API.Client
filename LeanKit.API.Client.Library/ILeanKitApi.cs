//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using LeanKit.API.Client.Library.TransferObjects;

namespace LeanKit.API.Client.Library
{
	public interface ILeanKitApi
	{
		IEnumerable<BoardListing> GetBoards();
		IEnumerable<BoardListing> ListNewBoards();
		Board GetBoard(long boardId);
		IEnumerable<Lane> GetBacklogLanes(long boardId);
		IEnumerable<HierarchicalLane> GetArchiveLanes(long boardId);
		IEnumerable<CardView> GetArchiveCards(long boardId);
		Board GetNewerIfExists(long boardId, int version);
		IEnumerable<BoardHistoryEvent> GetBoardHistorySince(long boardId, int version);
		CardView GetCard(long boardId, long cardId);
		Card GetCardByExternalId(long boardId, string externalCardId);
		BoardIdentifiers GetBoardIdentifiers(long boardId);
		long MoveCard(long boardId, long cardId, long toLaneId, int position, string wipOverrideReason);
		long MoveCardByExternalId(long boardId, string externalId, long toLaneId, int position, string wipOverrideReason);
		long MoveCard(long boardId, long cardId, long toLaneId, int position);
		CardAddResult AddCard(long boardId, Card newCard, string wipOverrideReason);
		CardAddResult AddCard(long boardId, Card newCard);
		IEnumerable<Card> AddCards(long boardId, IEnumerable<Card> newCards, string wipOverrideReason);
		IEnumerable<Card> AddCards(long boardId, IEnumerable<Card> newCards);
		CardUpdateResult UpdateCard(long boardId, Card updatedCard, string wipOverrideReason);
		CardUpdateResult UpdateCard(long boardId, Card updatedCard);
		CardUpdateResult UpdateCard(IDictionary<string, object> cardFieldUpdates);
		CardsUpdateResult UpdateCards(long boardId, IEnumerable<Card> updatedCards, string wipOverrideReason);
		CardsUpdateResult UpdateCards(long boardId, IEnumerable<Card> updatedCards);
		long DeleteCard(long boardId, long cardId);
		CardsDeleteResult DeleteCards(long boardId, IEnumerable<long> cardIds);
        CardDelegationResult DelegateCard(long cardId, long delegationBoardId);
        BoardUpdates CheckForUpdates(long boardId, int version);
		IEnumerable<Comment> GetComments(long boardId, long cardId);
		int PostComment(long boardId, long cardId, Comment comment);
		int PostCommentByExternalId(long boardId, string externalId, Comment comment);
		IEnumerable<CardEvent> GetCardHistory(long boardId, long cardId);
		IEnumerable<CardView> SearchCards(long boardId, SearchOptions options);
		IEnumerable<CardList> ListNewCards(long boardId);
		CardMoveBetweenBoardsResult MoveCardToAnotherBoard(long cardId, long destBoardId);
		User GetCurrentUser(long boardId);
		DrillThroughStatistics GetDrillThroughStatistics(long boardId, long cardId);

		Taskboard GetTaskboardById(long boardId, long taskboardId);
		Taskboard GetTaskboard(long boardId, long cardId);
		CardAddResult AddTask(long boardId, long cardId, Card newTask);
		CardAddResult AddTask(long boardId, long cardId, Card newTask, string wipOverrideReason);
		CardUpdateResult UpdateTask(long boardId, long cardId, Card updatedTask);
		CardUpdateResult UpdateTask(long boardId, long cardId, Card updatedTask, string wipOverrideReason);
		long DeleteTask(long boardId, long cardId, long taskId);
		CardMoveResult MoveTask(long boardId, long cardId, long taskId, long toLaneId, int position);
		CardMoveResult MoveTask(long boardId, long cardId, long taskId, long toLaneId, int position, string wipOverrideReason);

		long SaveAttachment(long boardId, long cardId, string fileName, string description, string mimeType, byte[] fileBytes);
		long DeleteAttachment(long boardId, long cardId, long attachmentId);
		Asset GetAttachment(long boardId, long cardId, long attachmentId);
		IEnumerable<Asset> GetAttachments(long boardId, long cardId);
		byte[] DownloadAttachment(long boardId, long attachmentId);

		#region Obsolete
	
		[Obsolete("Creating taskboards is no longer supported", true)]
		TaskboardCreateResult CreateTaskboard(long boardId, long containingCardId, TaskboardTemplateType templateType, long cardContextId);
		[Obsolete("Deleting taskboards is no longer supported", true)]
		TaskboardDeleteResult DeleteTaskboard(long boardId, long taskboardId);

		[Obsolete("Use AddTask instead")]
		CardAddResult AddTaskboardCard(long boardId, long taskboardId, Card newCard);
		[Obsolete("Use AddTask instead")]
		CardAddResult AddTaskboardCard(long boardId, long taskboardId, Card newCard, string wipOverrideReason);
		[Obsolete("Use UpdateTask instead")]
		CardUpdateResult UpdateTaskboardCard(long boardId, long taskboardId, Card updatedCard, string wipOverrideReason);
		[Obsolete("Use UpdateTask instead")]
		CardUpdateResult UpdateTaskboardCard(long boardId, long taskboardId, Card updatedCard);
		[Obsolete("Use DeleteTask instead")]
		long DeleteTaskboardCard(long boardId, long taskboardId, long cardId);
		[Obsolete("Use MoveTask instead")]
		CardMoveResult MoveTaskboardCard(long boardId, long taskboardId, long cardId, long toLaneId, int position,
			string wipOverrideReason);
		[Obsolete("Use MoveTask instead")]
		CardMoveResult MoveTaskboardCard(long boardId, long taskboardId, long cardId, long toLaneId, int position);

		#endregion


	}
}