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
		event EventHandler<ClientErrorEventArgs> ClientError;
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

		Taskboard GetTaskboard(long cardId);
		void AddTask(Card task, long cardId);
		void AddTask(Card task, long cardId, string wipOverrideReason);
		void UpdateTask(Card task, long cardId);
		void UpdateTask(Card task, long cardId, string wipOverrideReason);
		void DeleteTask(long taskId, long cardId);
		void MoveTask(long taskId, long cardId, long toLaneId, int position);
		void MoveTask(long taskId, long cardId, long toLaneId, int position, string wipOverrideReason);

		#region obsolete

		[Obsolete("Creating taskboards is no longer supported", true)]
		void CreateTaskboard(long cardId, TaskboardTemplateType templateType, long cardContextId);
		[Obsolete("Deleting taskboards is no longer supported", true)]
		void DeleteTaskboard(long cardId, long taskboardId);
		[Obsolete("Use AddTask instead")]
		void AddTaskboardCard(Card card, long taskboardId);
		[Obsolete("Use AddTask instead")]
		void AddTaskboardCard(Card card, long taskboardId, string wipOverrideReason);
		[Obsolete("Use UpdateTask instead")]
		void UpdateTaskboardCard(Card card, long taskboardId);
		[Obsolete("Use UpdateTask instead")]
		void UpdateTaskboardCard(Card card, long taskboardId, string wipOverrideReason);
		[Obsolete("Use DeleteTask instead")]
		void DeleteTaskboardCard(long cardId, long taskboardId);
		[Obsolete("Use MoveTask instead")]
		void MoveTaskboardCard(long cardId, long taskboardId, long toLaneId, int position, string wipOverrideReason);
		[Obsolete("Use MoveTask instead")]
		void MoveTaskboardCard(long cardId, long taskboardId, long toLaneId, int position);

		#endregion
	}
}