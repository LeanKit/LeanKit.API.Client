//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using LeanKit.API.Client.Library.TransferObjects;

namespace LeanKit.API.Client.Library
{
	public interface ILeanKitApi
	{
		IEnumerable<BoardListing> GetBoards();
		IEnumerable<BoardListing> ListNewBoards();
		Board GetBoard(long boardId);
		Taskboard GetTaskboard(long boardId, long taskboardId);
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
		BoardUpdates CheckForUpdates(long boardId, int version);
		IEnumerable<Comment> GetComments(long boardId, long cardId);
		int PostComment(long boardId, long cardId, Comment comment);
		int PostCommentByExternalId(long boardId, string externalId, Comment comment);
		IEnumerable<CardEvent> GetCardHistory(long boardId, long cardId);
		IEnumerable<CardView> SearchCards(long boardId, SearchOptions options);

		TaskboardCreateResult CreateTaskboard(long boardId, long containingCardId, TaskboardTemplateType templateType,
			long cardContextId);

		TaskboardDeleteResult DeleteTaskboard(long boardId, long taskboardId);
		CardAddResult AddTaskboardCard(long boardId, long taskboardId, Card newCard);
		CardAddResult AddTaskboardCard(long boardId, long taskboardId, Card newCard, string wipOverrideReason);
		CardUpdateResult UpdateTaskboardCard(long boardId, long taskboardId, Card updatedCard, string wipOverrideReason);
		CardUpdateResult UpdateTaskboardCard(long boardId, long taskboardId, Card updatedCard);
		long DeleteTaskboardCard(long boardId, long taskboardId, long cardId);

		CardMoveResult MoveTaskboardCard(long boardId, long taskboardId, long cardId, long toLaneId, int position,
			string wipOverrideReason);

		CardMoveResult MoveTaskboardCard(long boardId, long taskboardId, long cardId, long toLaneId, int position);
		IEnumerable<CardList> ListNewCards(long boardId);
		CardMoveBetweenBoardsResult MoveCardToAnotherBoard(long cardId, long destBoardId);
		User GetCurrentUser(long boardId);
	}
}