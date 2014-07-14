//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LeanKit.API.Client.Library.Enumerations;
using LeanKit.API.Client.Library.EventArguments;
using LeanKit.API.Client.Library.Exceptions;
using LeanKit.API.Client.Library.Extensions;
using LeanKit.API.Client.Library.TransferObjects;

namespace LeanKit.API.Client.Library
{
	public enum CheckForUpdatesLoopResult
	{
		Continue,
		Exit
	}

	/// <summary>
	///     This class represents a wrapper on top of the LeanKit API that creates a stateful
	///     representation of a LeanKit Board.  This will allow an integration to manage a board as
	///     though it is local to the customer's system.
	/// </summary>
	public class LeanKitIntegration : ILeanKitIntegration
	{
		private readonly ILeanKitApi _api;
		private readonly long _boardId;

		private readonly ReaderWriterLockSlim _boardLock = new ReaderWriterLockSlim();

		private readonly IntegrationSettings _integrationSettings;
		private Board _board;
		private bool _includeTaskboards;

		public LeanKitIntegration(long boardId, ILeanKitApi apiClient) : this(boardId, apiClient, new IntegrationSettings())
		{
		}

		public LeanKitIntegration(long boardId, ILeanKitApi apiClient, IntegrationSettings settings)
		{
			_integrationSettings = settings;
			_boardId = boardId;
			_api = apiClient;
			ShouldContinue = true;
		}

		public bool ShouldContinue { get; set; }

		public event EventHandler<BoardStatusCheckedEventArgs> BoardStatusChecked;
		public event EventHandler<BoardInfoRefreshedEventArgs> BoardInfoRefreshed;
		public event EventHandler<BoardChangedEventArgs> BoardChanged;
		public event EventHandler<ClientErrorEventArgs> ClientError;

		public void StartWatching()
		{
			StartWatching(false);
		}

		public void StartWatching(bool includeTaskboards)
		{
			_includeTaskboards = includeTaskboards;
			InitBoard();
		}

		private void InitBoard()
		{
			//setup the loop intervals
			//Probably need to do this on a different thread    
			do
			{
				try
				{
					_boardLock.EnterWriteLock();
					_board = _api.GetBoard(_boardId);
					//_boardIdentifiers = _api.GetBoardIdentifiers(_boardId);
				}
				finally
				{
					_boardLock.ExitWriteLock();
				}
			} while (SetupCheckForUpdatesLoop() != CheckForUpdatesLoopResult.Exit);
			// SetupBoardRefreshLoop();
		}

		#region Board Monitoring and Event Notifications

		public virtual void OnBoardStatusChecked(BoardStatusCheckedEventArgs eventArgs)
		{
			var eventToRaise = BoardStatusChecked;
			if (eventToRaise != null)
				eventToRaise(this, eventArgs);
		}

		public virtual void OnBoardRefresh(BoardInfoRefreshedEventArgs eventArgs)
		{
			var eventToRaise = BoardInfoRefreshed;
			if (eventToRaise != null)
				eventToRaise(this, eventArgs);
		}

		public virtual void OnBoardChanged(BoardChangedEventArgs eventArgs)
		{
			var eventToRaise = BoardChanged;
			if (eventToRaise != null)
				eventToRaise(this, eventArgs);
		}

		public virtual void OnClientError(ClientErrorEventArgs eventArgs)
		{
			var eventToRaise = ClientError;
			if (eventToRaise != null)
				eventToRaise(this, eventArgs);
		}

		private CheckForUpdatesLoopResult SetupCheckForUpdatesLoop()
		{
			const int pulse = 1000;
			var pollingInterval = (long) _integrationSettings.CheckForUpdatesIntervalSeconds*1000;
			
			var stopWatch = new System.Diagnostics.Stopwatch();
			stopWatch.Start();

			while (ShouldContinue)
			{
				if (!stopWatch.IsRunning) stopWatch.Restart();
			
				Thread.Sleep(pulse);
				
				if (stopWatch.ElapsedMilliseconds < pollingInterval) continue;

				try
				{
					stopWatch.Stop();

					//Now do the work
					var checkResults = _api.CheckForUpdates(_board.Id, (int)_board.Version);

					if (checkResults == null) continue;

					OnBoardStatusChecked(new BoardStatusCheckedEventArgs { HasChanges = checkResults.HasUpdates });

					if (!checkResults.HasUpdates) continue;

					try
					{
						_boardLock.EnterUpgradeableReadLock();

						var boardChangedEventArgs = new BoardChangedEventArgs();

						if (checkResults.Events.Any(x => x.RequiresBoardRefresh))
						{
							boardChangedEventArgs.BoardWasReloaded = true;
							OnBoardChanged(boardChangedEventArgs);
							return CheckForUpdatesLoopResult.Continue;
						}

						//Now we need to spin through and update the board
						//and create the information to event
						foreach (var boardEvent in checkResults.Events)
						{
							try
							{
								switch (GetEventType(boardEvent.EventType))
								{
									case EventType.CardCreation:
										var addCardEvent = CreateCardAddEvent(boardEvent, checkResults.AffectedLanes);
										if (addCardEvent != null) boardChangedEventArgs.AddedCards.Add(addCardEvent);
										break;
									case EventType.CardMove:
										var movedCardEvent = CreateCardMoveEvent(boardEvent, checkResults.AffectedLanes);
										if (movedCardEvent != null) boardChangedEventArgs.MovedCards.Add(movedCardEvent);
										break;
									case EventType.CardFieldsChanged:
										var changedFieldsEvent = CreateCardUpdateEvent(boardEvent, checkResults.AffectedLanes);
										if (changedFieldsEvent != null) boardChangedEventArgs.UpdatedCards.Add(changedFieldsEvent);
										break;
									case EventType.CardDeleted:
										boardChangedEventArgs.DeletedCards.Add(CreateCardDeletedEvent(boardEvent));
										break;
									case EventType.CardBlocked:
										if (boardEvent.IsBlocked)
											boardChangedEventArgs.BlockedCards.Add(CreateCardBlockedEvent(boardEvent,
												checkResults.AffectedLanes));
										else
											boardChangedEventArgs.UnBlockedCards.Add(CreateCardUnBlockedEvent(boardEvent,
												checkResults.AffectedLanes));
										break;
									case EventType.UserAssignment:
										if (boardEvent.IsUnassigning)
											boardChangedEventArgs.UnAssignedUsers.Add(CreateCardUserUnAssignmentEvent(boardEvent,
												checkResults.AffectedLanes));
										else
											boardChangedEventArgs.AssignedUsers.Add(CreateCardUserAssignmentEvent(boardEvent,
												checkResults.AffectedLanes));
										break;
									case EventType.CommentPost:
										boardChangedEventArgs.PostedComments.Add(CreateCommentPostedEvent(boardEvent));
										break;
									case EventType.WipOverride:
										boardChangedEventArgs.WipOverrides.Add(CreateWipOverrideEvent(boardEvent,
											checkResults.AffectedLanes));
										break;
									case EventType.UserWipOverride:
										boardChangedEventArgs.UserWipOverrides.Add(CreateUserWipOverrideEvent(boardEvent));
										break;
									case EventType.AttachmentChange:
										var attachmentEvent = CreateAttachmentEvent(boardEvent);
										if (attachmentEvent != null) boardChangedEventArgs.AttachmentChangedEvents.Add(attachmentEvent);
										break;

									case EventType.CardMoveToBoard:
										boardChangedEventArgs.CardMoveToBoardEvents.Add(CreateCardMoveToBoardEvent(boardEvent));
										break;

									case EventType.CardMoveFromBoard:
										boardChangedEventArgs.CardMoveFromBoardEvents.Add(CreateCardMoveFromBoardEvent(boardEvent));
										break;

									case EventType.BoardEdit:
										boardChangedEventArgs.BoardEditedEvents.Add(CreateBoardEditedEvent(boardEvent));
										boardChangedEventArgs.BoardStructureChanged = true;
										break;

									case EventType.BoardCardTypesChanged:
										boardChangedEventArgs.BoardCardTypesChangedEvents.Add(new BoardCardTypesChangedEvent(boardEvent.EventDateTime));
										boardChangedEventArgs.BoardStructureChanged = true;
										break;

									case EventType.BoardClassOfServiceChanged:
										boardChangedEventArgs.BoardClassOfServiceChangedEvents.Add(
											new BoardClassOfServiceChangedEvent(boardEvent.EventDateTime));
										boardChangedEventArgs.BoardStructureChanged = true;
										break;

									case EventType.Unrecognized:
										//Console.Beep();
										break;
								}
							}
							catch (Exception ex)
							{
								OnClientError(new ClientErrorEventArgs
								{
									Exception = ex,
									Message = "Error processing board change event. " + ex.Message
								});
							}
						}

						OnBoardChanged(boardChangedEventArgs);

						_boardLock.EnterWriteLock();
						try
						{
							//we need to check to see if there is a need to refresh the entire board
							//if so, we need to refresh the entire board and raise the board refreshed event
							if (!checkResults.RequiresRefesh())
							{
								//since the board does not require a refresh, then just change the effected lanes
								ApplyBoardChanges(checkResults.CurrentBoardVersion, checkResults.AffectedLanes);
							}
							else
							{
								_board = checkResults.NewPayload;
								OnBoardRefresh(new BoardInfoRefreshedEventArgs { FromBoardChange = true });
							}
						}
						catch (Exception ex)
						{
							OnClientError(new ClientErrorEventArgs
							{
								Exception = ex,
								Message = "Error applying board changes or raising board refresh."
							});
						}
						finally
						{
							_boardLock.ExitWriteLock();
						}
					}
					catch (Exception ex)
					{
						OnClientError(new ClientErrorEventArgs { Exception = ex, Message = "Error processing board events." });
					}
					finally
					{
						_boardLock.ExitUpgradeableReadLock();
					}
				}
				catch (Exception ex)
				{
					OnClientError(new ClientErrorEventArgs { Exception = ex, Message = "Error checking for board events." });
				}

			}
			stopWatch.Stop();

			return CheckForUpdatesLoopResult.Exit;
		}

		private void ApplyBoardChanges(int boardVersion, IEnumerable<Lane> affectedLanes)
		{
			_board.Version = boardVersion;

			foreach (Lane affectedLane in affectedLanes)
			{
				_board.UpdateLane(affectedLane);
			}
		}

		private static CardBlockedEvent CreateCardBlockedEvent(BoardHistoryEvent boardEvent, IEnumerable<Lane> affectedLanes)
		{
			//Get the effected lanes from the original board
			var card = affectedLanes.FindContainedCard(boardEvent.ToLaneId, boardEvent.CardId);

			return new CardBlockedEvent(boardEvent.EventDateTime, card, boardEvent.BlockedComment);
		}


		private static CardUnBlockedEvent CreateCardUnBlockedEvent(BoardHistoryEvent boardEvent,
			IEnumerable<Lane> affectedLanes)
		{
			//Get the effected lanes from the original board
			var card = affectedLanes.FindContainedCard(boardEvent.ToLaneId, boardEvent.CardId);

			return new CardUnBlockedEvent(boardEvent.EventDateTime, card, boardEvent.BlockedComment);
		}

		private CardUserAssignmentEvent CreateCardUserAssignmentEvent(BoardHistoryEvent boardEvent,
			IEnumerable<Lane> affectedLanes)
		{
			// Is the card on a taskboard?
			if (!_includeTaskboards && !_board.AllLanes().ContainsLane(boardEvent.ToLaneId))
				return null;

			try
			{
				var card = affectedLanes.FindContainedCard(boardEvent.ToLaneId, boardEvent.CardId);
				var assignedUser = _board.BoardUsers.FindUser(boardEvent.AssignedUserId);
				return new CardUserAssignmentEvent(boardEvent.EventDateTime, card, assignedUser);
			}
			catch (ItemNotFoundException ex)
			{
				throw new ItemNotFoundException(
					string.Format(
						"Unable to create Card User Assignment Event for board [{0}], lane [{1}], card [{2}], and user [{3}]. User count: {4}. {5}",
						_boardId, boardEvent.ToLaneId, boardEvent.CardId, boardEvent.AssignedUserId, _board.BoardUsers.Count(), ex.Message));
			}
		}

		private CardUserUnAssignmentEvent CreateCardUserUnAssignmentEvent(BoardHistoryEvent boardEvent,
			IEnumerable<Lane> affectedLanes)
		{
			// Is the card on a taskboard?
			if (!_includeTaskboards && !_board.AllLanes().ContainsLane(boardEvent.ToLaneId))
				return null;

			try
			{
				var card = affectedLanes.FindContainedCard(boardEvent.ToLaneId, boardEvent.CardId);
				var unAssignedUser = _board.BoardUsers.FindUser(boardEvent.AssignedUserId);

				return new CardUserUnAssignmentEvent(boardEvent.EventDateTime, card, unAssignedUser);
			}
			catch (ItemNotFoundException ex)
			{
				throw new ItemNotFoundException(
					string.Format(
						"Unable to create Card User Unassignment Event for board [{0}], lane [{1}], card [{2}], and user [{3}]. User count: {4}. {5}",
						_boardId, boardEvent.ToLaneId, boardEvent.CardId, boardEvent.AssignedUserId, _board.BoardUsers.Count(), ex.Message));
			}
		}

		private CommentPostedEvent CreateCommentPostedEvent(BoardHistoryEvent boardEvent)
		{
			//since no info in affectedLanes getting card from board
			Card affectedCard = _board.GetCardById(boardEvent.CardId);
			return new CommentPostedEvent(boardEvent.EventDateTime, affectedCard, boardEvent.CommentText);
		}

		private WipOverrideEvent CreateWipOverrideEvent(BoardHistoryEvent boardEvent, IEnumerable<Lane> affectedLanes)
		{
			try
			{
				Lane affectedLane = affectedLanes.FindLane(boardEvent.WipOverrideLane);
				return new WipOverrideEvent(boardEvent.EventDateTime, boardEvent.WipOverrideComment, affectedLane);
			}
			catch (ItemNotFoundException ex)
			{
				throw new ItemNotFoundException(
					string.Format("Unable to create Wip Override Event for board [{0}] and lane [{1}]. {2}", _boardId,
						boardEvent.WipOverrideLane, ex.Message));
			}
		}

		private UserWipOverrideEvent CreateUserWipOverrideEvent(BoardHistoryEvent boardEvent)
		{
			try
			{
				User affectedUser = _board.BoardUsers.FindUser(boardEvent.WipOverrideUser);
				return new UserWipOverrideEvent(boardEvent.EventDateTime, boardEvent.WipOverrideComment, affectedUser);
			}
			catch (ItemNotFoundException)
			{
				throw new ItemNotFoundException(
					string.Format("Unable to create User Wip Override Event for board [{0}] and user [{1}]", _boardId,
						boardEvent.WipOverrideUser));
			}
		}

		private AttachmentChangedEvent CreateAttachmentEvent(BoardHistoryEvent boardEvent)
		{
			// Is the card on a taskboard?
			if (!_board.AllLanes().ContainsLane(boardEvent.ToLaneId) && !_includeTaskboards)
				return null;

			var card = _board.AllLanes().FindContainedCard(boardEvent.ToLaneId, boardEvent.CardId);
			return new AttachmentChangedEvent(boardEvent.EventDateTime, card, boardEvent.FileName, boardEvent.CommentText,
				boardEvent.IsFileBeingDeleted);
		}

		private CardMoveToBoardEvent CreateCardMoveToBoardEvent(BoardHistoryEvent boardEvent)
		{
			return new CardMoveToBoardEvent(boardEvent.EventDateTime, boardEvent.CardId, boardEvent.ToLaneId);
		}

		private CardMoveFromBoardEvent CreateCardMoveFromBoardEvent(BoardHistoryEvent boardEvent)
		{
			return new CardMoveFromBoardEvent(boardEvent.EventDateTime, boardEvent.CardId, boardEvent.FromLaneId);
		}

		private CardDeletedEvent CreateCardDeletedEvent(BoardHistoryEvent boardEvent)
		{
			// Is the card being deleted from a taskboard?
			if (!_includeTaskboards && !_board.AllLanes().ContainsLane(boardEvent.ToLaneId))
				return null;

			var card = (_board.AllLanes().ContainsLane(boardEvent.ToLaneId))
				? _board.AllLanes().FindContainedCard(boardEvent.ToLaneId, boardEvent.CardId)
				: GetCard(boardEvent.CardId);

			return new CardDeletedEvent(boardEvent.EventDateTime, card);
		}

		private CardMoveEvent CreateCardMoveEvent(BoardHistoryEvent boardEvent, IEnumerable<Lane> affectedLanes)
		{
			try
			{
				// Is the card being moved in or to a taskboard?
				if (!_board.AllLanes().ContainsLane(boardEvent.ToLaneId) && !_includeTaskboards)
					return null;

				var fromLaneId = boardEvent.FromLaneId.GetValueOrDefault();
				var toLaneId = boardEvent.ToLaneId;
				var lanes = affectedLanes as IList<Lane> ?? affectedLanes.ToList();

				var fromLane = _board.AllLanes().ContainsLane(fromLaneId)
					? _board.AllLanes().FindLane(fromLaneId)
					: null;

				Lane toLane = null;
				CardView affectedCardView = null;
				if (lanes.ContainsLane(toLaneId))
				{
					toLane = lanes.FindLane(toLaneId);
					affectedCardView = toLane.Cards.FirstOrDefault(aCard => aCard.Id == boardEvent.CardId);
				}
				else if (_board.Archive.ContainsLane(toLaneId))
				{
					toLane = _board.Archive.FindLane(toLaneId);
					affectedCardView = GetCard(boardEvent.CardId).ToCardView();
				}

				// If fromLane or toLane are null, then the card is probably on a taskboard
				if (affectedCardView == null || (toLane == null && !_includeTaskboards)) return null;

				var card = affectedCardView.ToCard();
				return new CardMoveEvent(boardEvent.EventDateTime, fromLane, toLane, card);
			}
			catch (ItemNotFoundException ex)
			{
				throw new ItemNotFoundException(
					string.Format(
						"Unable to create Card Move Event for board [{0}], card [{1}], from lane [{2}] and to lane [{3}]. {4}", _boardId,
						boardEvent.CardId, boardEvent.FromLaneId.GetValueOrDefault(), boardEvent.ToLaneId, ex.Message));
			}
		}

		private CardAddEvent CreateCardAddEvent(BoardHistoryEvent boardEvent, IEnumerable<Lane> affectedLanes)
		{
			try
			{
				// Is the card being created on a taskboard?
				if (!_board.AllLanes().ContainsLane(boardEvent.ToLaneId) && !_includeTaskboards)
					return null;

				var lanes = affectedLanes as IList<Lane> ?? affectedLanes.ToList();
				var addedCard = lanes.ContainsCard(boardEvent.CardId)
					? lanes.FindContainedCard(boardEvent.ToLaneId, boardEvent.CardId)
					: GetCard(boardEvent.CardId);

				return new CardAddEvent(boardEvent.EventDateTime, addedCard);
			}
			catch (ItemNotFoundException ex)
			{
				throw new ItemNotFoundException(
					string.Format("Unable to create Card Add Event for board [{0}], card [{1}], and to lane [{2}]. {3}", _boardId,
						boardEvent.CardId, boardEvent.ToLaneId, ex.Message));
			}
		}

		private CardUpdateEvent CreateCardUpdateEvent(BoardHistoryEvent boardEvent, IEnumerable<Lane> affectedLanes)
		{
			try
			{
				// Is the card being updated on a taskboard?
				if (!_board.AllLanes().ContainsLane(boardEvent.ToLaneId) && !_includeTaskboards)
					return null;

				var originalCard = _board.GetCardById(boardEvent.CardId);

				if (originalCard == null)
					return null;

				var updatedCard =
					affectedLanes.FindContainedCard(boardEvent.ToLaneId, boardEvent.CardId);
				return new CardUpdateEvent(boardEvent.EventDateTime, originalCard, updatedCard);
			}
			catch (ItemNotFoundException ex)
			{
				throw new ItemNotFoundException(
					string.Format("Unable to create Card Update Event for board [{0}], card [{1}], and to lane [{2}]. {3}", _boardId,
						boardEvent.CardId, boardEvent.ToLaneId, ex.Message));
			}
		}

		private BoardEditedEvent CreateBoardEditedEvent(BoardHistoryEvent boardEvent)
		{
			return new BoardEditedEvent(boardEvent.EventDateTime, boardEvent.Message);
		}

		private static EventType GetEventType(string eventType)
		{
			eventType = eventType.Replace("LeanKit.Core.Events.", string.Empty).Replace("Event", string.Empty);

			EventType candidateEventType;
			return Enum.TryParse(eventType, true, out candidateEventType) ? candidateEventType : EventType.Unrecognized;
		}

		#endregion

		#region Pass through calls

		/* Pass through API */

		public virtual Card GetCard(long cardId)
		{
			Card card;
			_boardLock.EnterReadLock();
			try
			{
				card = _board.GetCardById(cardId);
			}
			finally
			{
				_boardLock.ExitReadLock();
			}

			// try getting card directly from the api
			if (card == null)
			{
				var c = _api.GetCard(_boardId, cardId);
				card = (c != null) ? c.ToCard() : null;
			}

			//try to find in archive, suppose it is not loaded
			if (card == null)
			{
				var archive = _api.GetArchiveCards(_boardId);
				var firstOrDefault = (archive != null) ? archive.FirstOrDefault(x => x.Id == cardId) : null;
				if (firstOrDefault != null) card = firstOrDefault.ToCard();
				//TODO: need to load the archive
				//Also, could be possible that the card is part of the cards older than 90 days
				//In that case we my need to do a search too.
			}

			if (card == null) throw new ItemNotFoundException();

			return card;
		}

		public Card GetCardByExternalId(long boardId, string externalCardId)
		{
			Card card;
			_boardLock.EnterReadLock();
			try
			{
				card = _board.GetCardByExternalId(externalCardId);
			}
			finally
			{
				_boardLock.ExitReadLock();
			}

			//try to find in archive, suppose it is not loaded
			if (card == null)
			{
				var archive = _api.GetArchiveCards(_boardId).ToList();
				var cardView = archive.FirstOrDefault(x => x.ExternalCardID == externalCardId);
				card = cardView == null ? null : cardView.ToCard();
				//TODO: need to load the archive
				//Also, could be possible that the card is part of the cards older than 90 days
				//In that case we my need to do a search too.
			}


			if (card == null) throw new ItemNotFoundException();

			return card;
		}

		public virtual void AddCard(Card card)
		{
			AddCard(card, string.Empty);
		}

		public void AddCard(Card card, string wipOverrideReason)
		{
			var results = string.IsNullOrEmpty(wipOverrideReason)
				? _api.AddCard(_boardId, card)
				: _api.AddCard(_boardId, card, wipOverrideReason);

			_boardLock.EnterWriteLock();
			try
			{
				ApplyBoardChanges(results.BoardVersion, new[] {results.Lane});
			}
			finally
			{
				_boardLock.ExitWriteLock();
			}
		}

		public virtual void UpdateCard(Card card)
		{
			UpdateCard(card, string.Empty);
		}

		public virtual void UpdateCard(Card card, string wipOverrideReason)
		{
			var results = string.IsNullOrEmpty(wipOverrideReason)
				? _api.UpdateCard(_boardId, card)
				: _api.UpdateCard(_boardId, card, wipOverrideReason);
			var cardView = results.CardDTO;
			var lane = _board.GetLaneById(cardView.LaneId);

			//TODO: handle the situation where a card in moved through the Update method

			_boardLock.EnterWriteLock();
			try
			{
				lane.UpdateCard(cardView);
				ApplyBoardChanges(results.BoardVersion, new[] {lane});
			}
			finally
			{
				_boardLock.ExitWriteLock();
			}
		}

		public virtual IEnumerable<Comment> GetComments(long cardId)
		{
			var card = _board.GetCardById(cardId);
			if (card.Comments != null) return card.Comments;
			var comments = _api.GetComments(_board.Id, cardId);
			card.Comments = new List<Comment>(comments);

			return card.Comments;
		}

		public virtual void PostComment(long cardId, Comment comment)
		{
			var cardComments = GetComments(cardId).ToList();
			cardComments.Add(comment);
			_api.PostComment(_board.Id, cardId, comment);
		}

		public virtual IEnumerable<CardEvent> GetCardHistory(long cardId)
		{
			//TODO:  What is the history has changed, might be better to just go and get the history everytime
			var card = _board.GetCardById(cardId);
			if (card.HistoryEvents != null) return card.HistoryEvents;
			var historyEvents = _api.GetCardHistory(_board.Id, cardId);
			card.HistoryEvents = new List<CardEvent>(historyEvents);

			return card.HistoryEvents;
		}

		public virtual void DeleteCard(long cardId)
		{
			var newVersion = _api.DeleteCard(_board.Id, cardId);

			_boardLock.EnterWriteLock();
			try
			{
				_board.ApplyCardDelete(cardId);
				_board.Version = newVersion;
			}
			finally
			{
				_boardLock.ExitWriteLock();
			}
		}

		public virtual void DeleteCards(IEnumerable<long> cardIds)
		{
			var cardIdList = cardIds as IList<long> ?? cardIds.ToList();
			var results = _api.DeleteCards(_board.Id, cardIdList);

			_boardLock.EnterWriteLock();
			try
			{
				foreach (var cardId in cardIdList)
				{
					_board.ApplyCardDelete(cardId);
				}

				_board.Version = results.BoardVersion;
			}
			finally
			{
				_boardLock.ExitReadLock();
			}
		}

		public virtual IEnumerable<Lane> GetArchive()
		{
			if (_board.Archive != null && _board.Archive.Count != 0) return _board.Archive;
			var hlanes = _api.GetArchiveLanes(_boardId);
			_boardLock.EnterWriteLock();
			try
			{
				_board.Archive = hlanes.GetFlatLanes().ToList();
			}
			finally
			{
				_boardLock.ExitWriteLock();
			}

			return _board.Archive;
		}

		public Board GetBoard()
		{
			_boardLock.EnterReadLock();
			try
			{
				return _board;
			}
			finally
			{
				_boardLock.ExitReadLock();
			}
		}

		public void MoveCard(long cardId, long toLaneId, int position, string wipOverrideReason)
		{
			var results = string.IsNullOrEmpty(wipOverrideReason)
				? _api.MoveCard(_board.Id, cardId, toLaneId, position)
				: _api.MoveCard(_board.Id, cardId, toLaneId, position, wipOverrideReason);


			_boardLock.EnterWriteLock();
			try
			{
				_board.ApplyCardMove(cardId, toLaneId, position);
				_board.Version = results;
			}
			finally
			{
				_boardLock.ExitWriteLock();
			}
		}

		public void MoveCard(long cardId, long toLaneId, int position)
		{
			MoveCard(cardId, toLaneId, position, string.Empty);
		}

		public IEnumerable<CardView> SearchCards(SearchOptions options)
		{
			//for now just pass through the Card Search
			return _api.SearchCards(_board.Id, options);
		}

		public Taskboard GetTaskboard(long cardId)
		{
			return _api.GetTaskboard(_boardId, cardId);
		}

		public void AddTask(Card task, long cardId)
		{
			AddTask(task, cardId, string.Empty);
		}

		public void AddTask(Card task, long cardId, string wipOverrideReason)
		{
			//var results = string.IsNullOrEmpty(wipOverrideReason)
			//	? _api.AddTask(_boardId, cardId, task)
			//	: _api.AddTask(_boardId, cardId, task, wipOverrideReason);

			//_boardLock.EnterWriteLock();
			//try {
			//	//TODO:  Figure out what to do for taskboards
			//	//ApplyBoardChanges(results.BoardVersion, new[] {results.Lane});
			//} finally {
			//	_boardLock.ExitWriteLock();
			//}			
		}

		public void UpdateTask(Card task, long cardId)
		{
			UpdateTask(task, cardId, string.Empty);
		}

		public void UpdateTask(Card task, long cardId, string wipOverrideReason)
		{
			//var results = string.IsNullOrEmpty(wipOverrideReason)
			//	? _api.UpdateTask(_boardId, cardId, task)
			//	: _api.UpdateTask(_boardId, cardId, task, wipOverrideReason);

			//TODO: Figure out how to handle taskboards
			//            CardView cardView = results.CardDTO;
			//            Lane lane = _board.GetLaneById(cardView.LaneId);
			//
			//TODO: handle the situation where a card in moved through the Update method
			//
			//            _boardLock.EnterWriteLock();
			//            try {
			//                lane.UpdateCard(cardView);
			//                ApplyBoardChanges(results.BoardVersion, new[] { lane });
			//            }
			//            finally {
			//                _boardLock.ExitWriteLock();
			//            }			
		}

		public void DeleteTask(long taskId, long cardId)
		{
			// var results = _api.DeleteTask(_boardId, cardId, taskId);
			//TODO: Figure out how to handle taskboards
		}

		public void MoveTask(long taskId, long cardId, long toLaneId, int position)
		{
			MoveTask(taskId, cardId, toLaneId, position, string.Empty);			
		}

		public void MoveTask(long taskId, long cardId, long toLaneId, int position, string wipOverrideReason)
		{
			// var results = _api.MoveTask(_boardId, cardId, taskId, toLaneId, position, wipOverrideReason);
			//TODO: Figure out how to handle taskboards
		}

		#region obsolete

		[Obsolete("Creating taskboards is no longer supported", true)]
		public void CreateTaskboard(long cardId, TaskboardTemplateType templateType, long cardContextId) {
			//_boardLock.EnterUpgradeableReadLock();
			//try
			//{
			//	var card = _board.GetCardById(cardId);
			//
			//	if (card.CardContexts.Any(cc => cc.Id == cardContextId))
			//	{
			//		throw new DuplicateItemException("A taskboard already exists for this Card Context.");
			//	}
			//
			//	_boardLock.EnterWriteLock();
			//	try
			//	{
			//		var result = _api.CreateTaskboard(_board.Id, cardId, templateType, cardContextId);
			//		_board.Version = result.BoardVersion;
			//	}
			//	finally
			//	{
			//		_boardLock.ExitWriteLock();
			//	}
			//}
			//finally
			//{
			//	_boardLock.ExitUpgradeableReadLock();
			//}
			throw new NotImplementedException("Creating taskboards is no longer supported");
		}

		[Obsolete("Deleting taskboards is no longer supported", true)]
		public void DeleteTaskboard(long cardId, long taskboardId) {
			//_boardLock.EnterUpgradeableReadLock();
			//try
			//{
			//	var card = _board.GetCardById(cardId);
			//
			//	if (card.CardContexts.All(cc => cc.TaskBoardId != taskboardId))
			//	{
			//		throw new ItemNotFoundException(string.Format("Could not find the Taskboard [{0}] for the Card [{1}].", taskboardId, cardId));
			//	}
			//
			//	_boardLock.EnterWriteLock();
			//	try
			//	{
			//		//TODO:  need to determine how to handle taskboards, how to apply the change
			//		var result = _api.DeleteTaskboard(_board.Id, taskboardId);
			//		_board.Version = result.BoardVersion;
			//	}
			//	finally
			//	{
			//		_boardLock.ExitWriteLock();
			//	}
			//}
			//finally
			//{
			//	_boardLock.ExitUpgradeableReadLock();
			//}
			throw new NotImplementedException("Creating taskboards is no longer supported");
		}

		[Obsolete("Use AddTask instead")]
		public void AddTaskboardCard(Card card, long taskboardId) {
			AddTaskboardCard(card, taskboardId, string.Empty);
		}

		[Obsolete("Use AddTask instead")]
		public void AddTaskboardCard(Card card, long taskboardId, string wipOverrideReason) {
			//var results = string.IsNullOrEmpty(wipOverrideReason)
			//	? _api.AddTaskboardCard(_boardId, taskboardId, card)
			//	: _api.AddTaskboardCard(_boardId, taskboardId, card, wipOverrideReason);

			//_boardLock.EnterWriteLock();
			//try {
			//	//TODO:  Figure out what to do for taskboards
			//	//ApplyBoardChanges(results.BoardVersion, new[] {results.Lane});
			//} finally {
			//	_boardLock.ExitWriteLock();
			//}
		}

		[Obsolete("Use UpdateTask instead")]
		public void UpdateTaskboardCard(Card card, long taskboardId) {
			UpdateTaskboardCard(card, taskboardId, string.Empty);
		}

		[Obsolete("Use UpdateTask instead")]
		public void UpdateTaskboardCard(Card card, long taskboardId, string wipOverrideReason) {
			//var results = string.IsNullOrEmpty(wipOverrideReason)
			//	? _api.UpdateTaskboardCard(_boardId, taskboardId, card)
			//	: _api.UpdateTaskboardCard(_boardId, taskboardId, card, wipOverrideReason);
			//TODO: Figure out how to handle taskboards
			//            CardView cardView = results.CardDTO;
			//            Lane lane = _board.GetLaneById(cardView.LaneId);
			//
			//TODO: handle the situation where a card in moved through the Update method
			//
			//            _boardLock.EnterWriteLock();
			//            try {
			//                lane.UpdateCard(cardView);
			//                ApplyBoardChanges(results.BoardVersion, new[] { lane });
			//            }
			//            finally {
			//                _boardLock.ExitWriteLock();
			//            }
		}

		[Obsolete("Use DeleteTask instead")]
		public void DeleteTaskboardCard(long cardId, long taskboardId) {
			throw new NotImplementedException();
		}

		[Obsolete("Use MoveTask instead")]
		public void MoveTaskboardCard(long cardId, long taskboardId, long toLaneId, int position, string wipOverrideReason)
		{
			throw new NotImplementedException();
		}

		[Obsolete("Use MoveTask instead")]
		public void MoveTaskboardCard(long cardId, long taskboardId, long toLaneId, int position)
		{
			throw new NotImplementedException();
		}

		#endregion

		#endregion
	}
}