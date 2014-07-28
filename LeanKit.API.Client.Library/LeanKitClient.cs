//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using LeanKit.API.Client.Library.TransferObjects;

namespace LeanKit.API.Client.Library
{
	/// <summary>
	///     The <see cref="LeanKit.API.Client.Library" /> provides a wrapper library designed to simplify the integration of
	///     external systems
	///     and utilities with your LeanKit Kanban account.  This library exposes both mechanisms that support interactions
	///     with LeanKit as
	///     well as a strongly typed object model representing the main entities within LeanKit.
	/// </summary>
	/// <remarks>
	///     This library exposes two primary ways to interact with LeanKit LeanKit.  These are the <see cref="LeanKitClient" />
	///     and
	///     <see cref="LeanKitIntegration" /> classes.  Both of these are explained below.
	///     <list type="bullet">
	///         <item>
	///             <term>
	///                 <see cref="LeanKitClient" />
	///             </term>
	///             <description>
	///                 This class exposes methods that interact directly with LeanKit API.  This class
	///                 exposed the same methods exposed through the REST API but hides the complexity of working with HTTP and
	///                 JSON.  This
	///                 class allows you to send commands and queries to LeanKit using the strongly-typed native objects.  This
	///                 class should
	///                 be used if you are building a simple stateless integration where you are not interested in interacting
	///                 with a LeanKit Board
	///                 over a period of time.  This class should also be used if you cannot support a long running service or
	///                 process.
	///             </description>
	///         </item>
	///         <item>
	///             <term>
	///                 <see cref="LeanKitIntegration" />
	///             </term>
	///             <description>
	///                 This class is designed to be used in stateful implementations where you plan to interact with a LeanKit
	///                 Board and react to
	///                 change events within a the board.  This class monitors the changes to the board and raises event as a
	///                 result of other users
	///                 interacting with the board.  This prevents custom integrations from having to implement the complexity
	///                 of polling for changes.
	///                 In addition, this class exposes the same command and query methods that are exposed by the
	///                 <see cref="LeanKitClient" /> class.
	///                 Because this implementation is stateful, many of these queries and commands can be optimized and
	///                 provide more powerful validation.
	///                 This class is designed to retrieve and hold a reference to the board and therefore should not be
	///                 instantiated numerous times.
	///                 Each instantiation will be associated to a single LeanKit Board.  You will need to create multiple
	///                 instances if you need to
	///                 interact with more than one Board.
	///             </description>
	///         </item>
	///     </list>
	///     The library will require the HostName(or URL) used by your organization to connect to LeanKit Kanban (ex. -
	///     https://acme.leankit.com)
	///     as well as valid account credentials (UserName and Password) used for the organization.
	/// </remarks>
	[CompilerGenerated]
	internal class NamespaceDoc
	{
	}

	/// <summary>
	///     This class wraps the raw API of LeanKit and insulates the consumer of the LeanKit API
	///     from many of the lower level details of working with the API. This class differs from
	///     the LeanKitApi class because of it's composition-based approach, because it implements
	///     ILeanKitClient (can be initialized), and because the logic of communicating with the
	///     actual REST server is abstracted behind the IRestCommandProcessor.
	/// </summary>
	public class LeanKitClient : ILeanKitApi, ILeanKitClient
	{
		private const string DefaultOverrideReason = "WIP Override performed by external system";
		private readonly IRestCommandProcessor _restCommandProcessor;
		private LeanKitAccountAuth _accountAuth;

		public LeanKitClient(IRestCommandProcessor restCommandProcessor)
		{
			_restCommandProcessor = restCommandProcessor;
		}

		#region ILeanKitApi Members

		#region Organization Methods

		public IEnumerable<BoardListing> GetBoards()
		{
			const string resource = "/Kanban/API/Boards";
			return _restCommandProcessor.Get<List<BoardListing>>(_accountAuth, resource);
		}

		public IEnumerable<BoardListing> ListNewBoards()
		{
			const string resource = "/Kanban/API/ListNewBoards";
			return _restCommandProcessor.Get<List<BoardListing>>(_accountAuth, resource);
		}

		#endregion

		#region Board Methods

		public Board GetBoard(long boardId)
		{
			var resource = string.Format("/Kanban/API/Boards/{0}", boardId);
			return _restCommandProcessor.Get<Board>(_accountAuth, resource);
		}

		public IEnumerable<Lane> GetBacklogLanes(long boardId)
		{
			var resource = string.Format("/Kanban/Api/Board/{0}/Backlog", boardId);
			return _restCommandProcessor.Get<List<Lane>>(_accountAuth, resource);
		}

		public IEnumerable<HierarchicalLane> GetArchiveLanes(long boardId)
		{
			var resource = string.Format("/Kanban/Api/Board/{0}/Archive", boardId);
			return _restCommandProcessor.Get<List<HierarchicalLane>>(_accountAuth, resource);
		}

		public IEnumerable<CardView> GetArchiveCards(long boardId)
		{
			var resource = string.Format("/Kanban/Api/Board/{0}/ArchiveCards", boardId);
			return _restCommandProcessor.Get<List<CardView>>(_accountAuth, resource);
		}

		public Board GetNewerIfExists(long boardId, int version)
		{
			var resource = string.Format("/Kanban/Api/Board/{0}/BoardVersion/{1}/GetNewerIfExists", boardId, version);
			return _restCommandProcessor.Get<Board>(_accountAuth, resource);
		}

		public IEnumerable<BoardHistoryEvent> GetBoardHistorySince(long boardId, int version)
		{
			var resource = string.Format("/Kanban/Api/Board/{0}/BoardVersion/{1}/GetBoardHistorySince", boardId, version);
			return _restCommandProcessor.Get<List<BoardHistoryEvent>>(_accountAuth, resource);
		}

		public CardView GetCard(long boardId, long cardId)
		{
			var resource = string.Format("/Kanban/Api/Board/{0}/GetCard/{1}", boardId, cardId);
			return _restCommandProcessor.Get<CardView>(_accountAuth, resource);
		}

		public Card GetCardByExternalId(long boardId, string externalCardId)
		{
			var resource = string.Format("/Kanban/Api/Board/{0}/GetCardByExternalId/{1}", boardId, externalCardId);
			var cards = _restCommandProcessor.Get<List<Card>>(_accountAuth, resource);
			return cards != null ? cards.FirstOrDefault() : null;
		}

		public BoardIdentifiers GetBoardIdentifiers(long boardId)
		{
			var resource = string.Format("/Kanban/Api/Board/{0}/GetBoardIdentifiers", boardId);
			return _restCommandProcessor.Get<BoardIdentifiers>(_accountAuth, resource);
		}

		public long MoveCard(long boardId, long cardId, long toLaneId, int position, string wipOverrideReason)
		{
			var resource = "/Kanban/Api/Board/" + boardId + "/MoveCardWithWipOverride/" + cardId + "/Lane/" + toLaneId +
			               "/Position/" + position;
			return _restCommandProcessor.Post<long>(_accountAuth, resource,
				new {Comment = (string.IsNullOrEmpty(wipOverrideReason) ? DefaultOverrideReason : wipOverrideReason)});
		}

		public long MoveCardByExternalId(long boardId, string externalId, long toLaneId, int position,
			string wipOverrideReason)
		{
			var resource = "/Kanban/Api/Board/" + boardId + "/MoveCardByExternalId/" + Uri.EscapeDataString(externalId) +
			               "/Lane/" + toLaneId + "/Position/" + position;
			return _restCommandProcessor.Post<long>(_accountAuth, resource,
				new {Comment = (string.IsNullOrEmpty(wipOverrideReason) ? DefaultOverrideReason : wipOverrideReason)});
		}

		public long MoveCard(long boardId, long cardId, long toLaneId, int position)
		{
			return MoveCard(boardId, cardId, toLaneId, position, DefaultOverrideReason);
		}

		public CardAddResult AddCard(long boardId, Card newCard)
		{
			return AddCard(boardId, newCard, DefaultOverrideReason);
		}

		public CardAddResult AddCard(long boardId, Card newCard, string wipOverrideReason)
		{
			var resource = string.Format("/Kanban/Api/Board/{0}/AddCardWithWipOverride/Lane/{1}/Position/{2}",
				boardId, newCard.LaneId, newCard.Index);
			newCard.UserWipOverrideComment = (string.IsNullOrEmpty(wipOverrideReason) ? DefaultOverrideReason : wipOverrideReason);
			return _restCommandProcessor.Post<CardAddResult>(_accountAuth, resource, newCard);
		}

		public IEnumerable<Card> AddCards(long boardId, IEnumerable<Card> newCards)
		{
			return AddCards(boardId, newCards, DefaultOverrideReason);
		}

		public IEnumerable<Card> AddCards(long boardId, IEnumerable<Card> newCards, string wipOverrideReason)
		{
			var resource = "/Kanban/Api/Board/" + boardId + "/AddCards?wipOverrideComment=" +
			               (string.IsNullOrEmpty(wipOverrideReason) ? DefaultOverrideReason : wipOverrideReason);
			return _restCommandProcessor.Post<List<Card>>(_accountAuth, resource, newCards);
		}

		public CardUpdateResult UpdateCard(long boardId, Card updatedCard)
		{
			return UpdateCard(boardId, updatedCard, DefaultOverrideReason);
		}

		public CardUpdateResult UpdateCard(long boardId, Card updatedCard, string wipOverrideReason)
		{
			var resource = "/Kanban/Api/Board/" + boardId + "/UpdateCardWithWipOverride";
			updatedCard.UserWipOverrideComment = wipOverrideReason;
			return _restCommandProcessor.Post<CardUpdateResult>(_accountAuth, resource, updatedCard);
		}

		public CardUpdateResult UpdateCard(IDictionary<string, object> cardFieldUpdates)
		{
			const string resource = "/Kanban/Api/Card/Update";
			return _restCommandProcessor.Post<CardUpdateResult>(_accountAuth, resource, cardFieldUpdates);
		}

		public CardsUpdateResult UpdateCards(long boardId, IEnumerable<Card> updatedCards)
		{
			return UpdateCards(boardId, updatedCards, DefaultOverrideReason);
		}

		public CardsUpdateResult UpdateCards(long boardId, IEnumerable<Card> updatedCards, string wipOverrideReason)
		{
			var resource = "/Kanban/Api/Board/" + boardId + "/UpdateCards?wipOverrideComment=" +
			               (string.IsNullOrEmpty(wipOverrideReason) ? DefaultOverrideReason : wipOverrideReason);
			return _restCommandProcessor.Post<CardsUpdateResult>(_accountAuth, resource, updatedCards);
		}

		public long DeleteCard(long boardId, long cardId)
		{
			var resource = "/Kanban/Api/Board/" + boardId + "/DeleteCard/" + cardId;
			return _restCommandProcessor.Post<long>(_accountAuth, resource);
		}

		public CardsDeleteResult DeleteCards(long boardId, IEnumerable<long> cardIds)
		{
			var resource = "/Kanban/Api/Board/" + boardId + "/DeleteCards";
			return _restCommandProcessor.Post<CardsDeleteResult>(_accountAuth, resource, cardIds);
		}

		public CardDelegationResult DelegateCard(long cardId, long delegationBoardId)
		{
			var resource = "Kanban/API/Card/Delegate/" + cardId + "/" + delegationBoardId;
			return _restCommandProcessor.Post<CardDelegationResult>(_accountAuth, resource);
		}

		public BoardUpdates CheckForUpdates(long boardId, int version)
		{
			var resource = "/Kanban/Api/Board/" + boardId + "/BoardVersion/" + version + "/CheckForUpdates";
			return _restCommandProcessor.Get<BoardUpdates>(_accountAuth, resource);
		}

		public IEnumerable<Comment> GetComments(long boardId, long cardId)
		{
			var resource = "/Kanban/Api/Card/GetComments/" + boardId + "/" + cardId;
			return _restCommandProcessor.Get<List<Comment>>(_accountAuth, resource);
		}

		public int PostComment(long boardId, long cardId, Comment comment)
		{
			var resource = "/Kanban/Api/Card/SaveComment/" + boardId + "/" + cardId;
			return _restCommandProcessor.Post<int>(_accountAuth, resource, comment);
		}

		public int PostCommentByExternalId(long boardId, string externalId, Comment comment)
		{
			var resource = "/Kanban/Api/Card/SaveCommentByExternalId/" + boardId + "/" + Uri.EscapeDataString(externalId);
			return _restCommandProcessor.Post<int>(_accountAuth, resource, comment);
		}

		public IEnumerable<CardEvent> GetCardHistory(long boardId, long cardId)
		{
			var resource = "/Kanban/Api/Card/History/" + boardId + "/" + cardId;
			return _restCommandProcessor.Get<List<CardEvent>>(_accountAuth, resource);
		}

		public IEnumerable<CardView> SearchCards(long boardId, SearchOptions options)
		{
			var resource = "/Kanban/Api/Board/" + boardId + "/SearchCards";
			return _restCommandProcessor.Post<PaginationResult<CardView>>(_accountAuth, resource, options).Results;
		}

		public IEnumerable<CardList> ListNewCards(long boardId)
		{
			var resource = string.Format("/Kanban/Api/Board/{0}/ListNewCards",
				boardId);
			return _restCommandProcessor.Get<List<CardList>>(_accountAuth, resource);
		}

		public CardMoveBetweenBoardsResult MoveCardToAnotherBoard(long cardId, long destBoardId)
		{
			//http://kanban-cibuild.leankit.local/kanban/API/Card/MoveCardToAnotherBoard/7/101
			var resource = string.Format("/Kanban/Api/Card/MoveCardToAnotherBoard/{0}/{1}", cardId, destBoardId);
			return _restCommandProcessor.Post<CardMoveBetweenBoardsResult>(_accountAuth, resource);
		}

		public DrillThroughStatistics GetDrillThroughStatistics(long boardId, long cardId)
		{
			var resource = string.Format("/Kanban/Api/Card/{0}/GetDrillThroughStatistics/{1}", boardId, cardId);
			return _restCommandProcessor.Get<DrillThroughStatistics>(_accountAuth, resource);
		}

		#endregion

		#region Taskboard methods

		public Taskboard GetTaskboardById(long boardId, long taskboardId)
		{
			var resource = "/Kanban/API/Board/" + boardId + "/TaskBoard/" + taskboardId + "/Get/";
			return _restCommandProcessor.Get<Taskboard>(_accountAuth, resource);
		}

		public Taskboard GetTaskboard(long boardId, long cardId)
		{
			var resource = "/Kanban/API/v1/Board/" + boardId + "/Card/" + cardId + "/Taskboard";
			return _restCommandProcessor.Get<Taskboard>(_accountAuth, resource);
		}

		public CardAddResult AddTask(long boardId, long cardId, Card newTask)
		{
			return AddTask(boardId, cardId, newTask, DefaultOverrideReason);
		}

		public CardAddResult AddTask(long boardId, long cardId, Card newTask, string wipOverrideReason)
		{
			var resource = string.Format("/Kanban/Api/v1/Board/{0}/Card/{1}/Tasks/Lane/{2}/Position/{3}",
				boardId, cardId, newTask.LaneId, newTask.Index);
			newTask.UserWipOverrideComment = wipOverrideReason;
			return _restCommandProcessor.Post<CardAddResult>(_accountAuth, resource, newTask);
		}

		public CardUpdateResult UpdateTask(long boardId, long cardId, Card updatedTask)
		{
			return UpdateTask(boardId, cardId, updatedTask, DefaultOverrideReason);
		}

		public CardUpdateResult UpdateTask(long boardId, long cardId, Card updatedTask, string wipOverrideReason)
		{
			var resource = string.Format("/Kanban/Api/v1/Board/{0}/Update/Card/{1}/Tasks/{2}", boardId, cardId, updatedTask.Id);
			updatedTask.UserWipOverrideComment = wipOverrideReason;
			return _restCommandProcessor.Post<CardUpdateResult>(_accountAuth, resource, updatedTask);
		}

		public long DeleteTask(long boardId, long cardId, long taskId)
		{
			var resource = string.Format("/Kanban/Api/v1/Board/{0}/Delete/Card/{1}/Tasks/{2}", boardId, cardId, taskId);
			return _restCommandProcessor.Post<long>(_accountAuth, resource);
		}

		public BoardUpdates CheckCardForTaskUpdates(long boardId, long cardId, long boardVersion)
		{
			var resource = string.Format("/Kanban/Api/v1/Board/{0}/Card/{1}/Tasks/BoardVersion/{2}",
				boardId, cardId, boardVersion);
			return _restCommandProcessor.Get<BoardUpdates>(_accountAuth, resource);
		}

		public CardMoveResult MoveTask(long boardId, long cardId, long taskId, long toLaneId, int position)
		{
			return MoveTask(boardId, cardId, taskId, toLaneId, position, DefaultOverrideReason);
		}

		public CardMoveResult MoveTask(long boardId, long cardId, long taskId, long toLaneId, int position,
			string wipOverrideReason)
		{
			var resource =
				string.Format("/Kanban/Api/v1/Board/{0}/Move/Card/{1}/Tasks/{2}/Lane/{3}/Position/{4}", boardId,
					cardId, taskId, toLaneId, position);
			return _restCommandProcessor.Post<CardMoveResult>(_accountAuth, resource,
				new {Comment = (string.IsNullOrEmpty(wipOverrideReason) ? DefaultOverrideReason : wipOverrideReason)});
		}

		#region Obsolete

		[Obsolete("Creating taskboards is no longer supported", true)]
		public TaskboardCreateResult CreateTaskboard(long boardId, long containingCardId, TaskboardTemplateType templateType,
			long cardContextId)
		{
			//var cmd = new CreateTaskBoardCommand
			//{
			//	BoardId = boardId,
			//	CardContextId = cardContextId,
			//	ContainingCardId = containingCardId,
			//	TemplateId = (long) templateType
			//};

			//var resource = "/Kanban/Api/TaskBoard/Create";
			//return _restCommandProcessor.Post<TaskboardCreateResult>(_accountAuth, resource, cmd);

			throw new NotImplementedException("Creating taskboards is no longer supported");
		}

		[Obsolete("Deleting taskboards is no longer supported", true)]
		public TaskboardDeleteResult DeleteTaskboard(long boardId, long taskBoardId)
		{
			//var resource = string.Format("/Kanban/API/Board/{0}/TaskBoard/{1}/Delete/", boardId, taskBoardId);
			//return _restCommandProcessor.Post<TaskboardDeleteResult>(_accountAuth, resource);

			throw new NotImplementedException("Deleting taskboards is no longer supported");
		}

		[Obsolete("Use AddTask instead.")]
		public CardAddResult AddTaskboardCard(long boardId, long taskboardId, Card newCard)
		{
			return AddTaskboardCard(boardId, taskboardId, newCard, DefaultOverrideReason);
		}

		[Obsolete("Use AddTask instead.")]
		public CardAddResult AddTaskboardCard(long boardId, long taskboardId, Card newCard, string wipOverrideReason)
		{
			var resource =
				string.Format("/Kanban/Api/Board/{0}/TaskBoard/{1}/AddCardWithWipOverrideLite/Lane/{2}/Position/{3}",
					boardId, taskboardId, newCard.LaneId, newCard.Index);
			newCard.UserWipOverrideComment = wipOverrideReason;
			return _restCommandProcessor.Post<CardAddResult>(_accountAuth, resource, newCard);
		}

		[Obsolete("Use UpdateTask instead")]
		public CardUpdateResult UpdateTaskboardCard(long boardId, long taskboardId, Card updatedCard, string wipOverrideReason)
		{
			var resource = string.Format("/Kanban/Api/Board/{0}/TaskBoard/{1}/UpdateCardWithWipOverrideLite", boardId,
				taskboardId);
			updatedCard.UserWipOverrideComment = wipOverrideReason;
			return _restCommandProcessor.Post<CardUpdateResult>(_accountAuth, resource, updatedCard);
		}

		[Obsolete("Use UpdateTask instead")]
		public CardUpdateResult UpdateTaskboardCard(long boardId, long taskboardId, Card updatedCard)
		{
			return UpdateTaskboardCard(boardId, taskboardId, updatedCard, DefaultOverrideReason);
		}

		[Obsolete("Use DeleteTask instead")]
		public long DeleteTaskboardCard(long boardId, long taskboardId, long cardId)
		{
			var resource = string.Format("/Kanban/Api/Board/{0}/TaskBoard/{1}/DeleteLite/{2}", boardId, taskboardId, cardId);
			return _restCommandProcessor.Post<long>(_accountAuth, resource);
		}

		[Obsolete("Use CheckCardForTaskUpdates instead")]
		public BoardUpdates CheckForTaskboardUpdates(long boardId, long taskBoardId, long taskBoardVersion)
		{
			var resource = string.Format("/Kanban/Api/Board/{0}/TaskBoard/{1}/BoardVersion/{2}/CheckForUpdates",
				boardId, taskBoardId, taskBoardVersion);
			return _restCommandProcessor.Get<BoardUpdates>(_accountAuth, resource);
		}

		[Obsolete("Use MoveTask instead")]
		public CardMoveResult MoveTaskboardCard(long boardId, long taskboardId, long cardId, long toLaneId, int position,
			string wipOverrideReason)
		{
			var resource =
				string.Format("/Kanban/Api/Board/{0}/TaskBoard/{1}/MoveCardWithWipOverrideLite/{2}/Lane/{3}/Position/{4}", boardId,
					taskboardId, cardId, toLaneId, position);
			return _restCommandProcessor.Post<CardMoveResult>(_accountAuth, resource,
				new {Comment = (string.IsNullOrEmpty(wipOverrideReason) ? DefaultOverrideReason : wipOverrideReason)});
		}

		[Obsolete("Use MoveTask instead")]
		public CardMoveResult MoveTaskboardCard(long boardId, long taskboardId, long cardId, long toLaneId, int position)
		{
			return MoveTaskboardCard(boardId, taskboardId, cardId, toLaneId, position, DefaultOverrideReason);
		}

		#endregion

		#endregion

		#region Attachment Methods

		public long SaveAttachment(long boardId, long cardId, string fileName, string description, string mimeType, byte[] fileBytes)
		{
			var resource = string.Format("/kanban/api/card/SaveAttachment/{0}/{1}", boardId, cardId);
			var parameters = new Dictionary<string, object> {{"Description", description}, {"Id", 0}};
			return _restCommandProcessor.PostFile<long>(_accountAuth, resource, parameters, fileName, mimeType, fileBytes);			
		}

		public long DeleteAttachment(long boardId, long cardId, long attachmentId)
		{
			var resource = string.Format("/Kanban/Api/Card/DeleteAttachment/{0}/{1}/{2}", boardId, cardId, attachmentId);
			return _restCommandProcessor.Post<long>(_accountAuth, resource);
		}

		public Asset GetAttachment(long boardId, long cardId, long attachmentId)
		{
			var resource = string.Format("/Kanban/Api/Card/GetAttachments/{0}/{1}/{2}", boardId, cardId, attachmentId);
			return _restCommandProcessor.Get<Asset>(_accountAuth, resource);
		}

		public IEnumerable<Asset> GetAttachments(long boardId, long cardId)
		{
			var resource = string.Format("/Kanban/Api/Card/GetAttachments/{0}/{1}", boardId, cardId);
			return _restCommandProcessor.Get<List<Asset>>(_accountAuth, resource);
		}

		#endregion

		#region User Methods

		public User GetCurrentUser(long boardId)
		{
			var resource = "/Api/User/GetCurrentUserSettings/" + boardId;
			return _restCommandProcessor.Get<User>(_accountAuth, resource);
		}

		#endregion

		#endregion

		#region ILeanKitClient Members

		public ILeanKitApi Initialize(LeanKitAccountAuth accountAuth)
		{
			_accountAuth = accountAuth;
			return this;
		}

		#endregion
	}
}