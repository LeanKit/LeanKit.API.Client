//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using LeanKit.API.Client.Library.EventArguments;
using LeanKit.API.Client.Library.TransferObjects;

namespace LeanKit.API.Client.Library
{
	public interface ILeanKitIntegration
	{
		bool ShouldContinue { get; set; }
		event EventHandler<BoardStatusCheckedEventArgs> BoardStatusChecked;
		event EventHandler<BoardInfoRefreshedEventArgs> BoardInfoRefreshed;
		event EventHandler<BoardChangedEventArgs> BoardChanged;
		void StartWatching();
		void OnBoardStatusChecked(BoardStatusCheckedEventArgs eventArgs);
		void OnBoardRefresh(BoardInfoRefreshedEventArgs eventArgs);
		void OnBoardChanged(BoardChangedEventArgs eventArgs);
		Card GetCard(long cardId);
		Card GetCardByExternalId(long boardId, string externalCardId);
		void AddCard(Card card);
		void AddCard(Card card, string wipOverrideReason);
		void UpdateCard(Card card);
		void UpdateCard(Card card, string wipOverrideReason);
		IEnumerable<Comment> GetComments(long cardId);
		void PostComment(long cardId, Comment comment);
		IEnumerable<CardEvent> GetCardHistory(long cardId);
		void DeleteCard(long cardId);
		void DeleteCards(IEnumerable<long> cardIds);
		IEnumerable<Lane> GetArchive();
		Board GetBoard();
		void MoveCard(long cardId, long toLaneId, int position, string wipOverrideReason);
		void MoveCard(long cardId, long toLaneId, int position);
		IEnumerable<CardView> SearchCards(SearchOptions options);
		void CreateTaskboard(long cardId, TaskboardTemplateType templateType, long cardContextId);
		void DeleteTaskboard(long cardId, long taskboardId);
		void AddTaskboardCard(Card card, long taskboardId);
		void AddTaskboardCard(Card card, long taskboardId, string wipOverrideReason);
		void UpdateTaskboardCard(Card card, long taskboardId);
		void UpdateTaskboardCard(Card card, long taskboardId, string wipOverrideReason);
		void DeleteTaskboardCard(long cardId, long taskboardId);
		void MoveTaskboardCard(long cardId, long taskboardId, long toLaneId, int position, string wipOverrideReason);
		void MoveTaskbordCard(long cardId, long taskboardId, long toLaneId, int position);
	}
}