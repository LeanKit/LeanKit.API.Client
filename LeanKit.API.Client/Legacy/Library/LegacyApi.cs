using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeanKit.API.Legacy.Library.TransferObjects;
using LeanKit.Extensions;

namespace LeanKit.API.Legacy.Library
{
	public class LegacyApi
	{
		private const string DefaultOverrideReason = "WIP Override performed by external system";
		private readonly Client _client;
		public LegacyApi(Client client)
		{
			_client = client;
		}

		public async Task<IEnumerable<BoardListing>> GetBoards()
		{
			const string resource = "/Kanban/API/Boards";
            return await _client.LegacyRequest<List<BoardListing>>(resource);
		}

		public async Task<IEnumerable<BoardListing>> ListNewBoards()
		{
			const string resource = "/Kanban/API/ListNewBoards";
			return await _client.LegacyRequest<List<BoardListing>>(resource);
		}

		public async Task<TransferObjects.Board> GetBoard(long boardId)
		{
			var resource = $"/Kanban/API/Boards/{boardId}";
			return await _client.LegacyRequest<TransferObjects.Board>(resource);
		}

		public async Task<IEnumerable<Lane>> GetBacklogLanes(long boardId)
		{
			var resource = $"/Kanban/Api/Board/{boardId}/Backlog";
			return await _client.LegacyRequest<List<Lane>>(resource);
		}

		public async Task<IEnumerable<HierarchicalLane>> GetArchiveLanes(long boardId)
		{
			var resource = $"/Kanban/Api/Board/{boardId}/Archive";
			return await _client.LegacyRequest<List<HierarchicalLane>>(resource);
		}

		public async Task<IEnumerable<CardView>> GetArchiveCards(long boardId)
		{
			var resource = $"/Kanban/Api/Board/{boardId}/ArchiveCards";
			return await _client.LegacyRequest<List<CardView>>(resource);
		}

		public async Task<TransferObjects.Board> GetNewerIfExists(long boardId, long version)
		{
			var resource = $"/Kanban/Api/Board/{boardId}/BoardVersion/{version}/GetNewerIfExists";
			return await _client.LegacyRequest<TransferObjects.Board>(resource);
		}

		public async Task<IEnumerable<BoardHistoryEvent>> GetBoardHistorySince(long boardId, long version)
		{
			var resource = $"/Kanban/Api/Board/{boardId}/BoardVersion/{version}/GetBoardHistorySince";
			return await _client.LegacyRequest<List<BoardHistoryEvent>>(resource);
		}

		public async Task<BoardUpdates> CheckForUpdates(long boardId, long version)
		{
			var resource = $"/Kanban/Api/Board/{boardId}/BoardVersion/{version}/CheckForUpdates";
			return await _client.LegacyRequest<BoardUpdates>(resource);
		}

		public async Task<BoardIdentifiers> GetBoardIdentifiers(long boardId)
		{
			var resource = $"/Kanban/Api/Board/{boardId}/GetBoardIdentifiers";
			return await _client.LegacyRequest<BoardIdentifiers>(resource);
		}

		public async Task<CardView> GetCard(long boardId, long cardId)
		{
			var resource = $"/Kanban/Api/Board/{boardId}/GetCard/{cardId}";
			return await _client.LegacyRequest<CardView>(resource);
		}

		public async Task<TransferObjects.Card> GetCardByExternalId(long boardId, string externalCardId)
		{
			var resource = $"/Kanban/Api/Board/{boardId}/GetCardByExternalId/{externalCardId}";
            var cards = await _client.LegacyRequest<List<TransferObjects.Card>>(resource);
			return cards != null ? cards.FirstOrDefault() : null;
		}

		public async Task<long> MoveCard(long boardId, long cardId, long toLaneId, int position, string wipOverrideReason = DefaultOverrideReason)
		{
            var body = new { Comment = (string.IsNullOrEmpty(wipOverrideReason) ? DefaultOverrideReason : wipOverrideReason) }.ToJSON();
			var resource = $"/Kanban/Api/Board/{boardId}/MoveCardWithWipOverride/{cardId}/Lane/{toLaneId}/Position/{position}";
            return await _client.LegacyRequest<long>(resource, "post", body);
		}

		public async Task<long> MoveCardByExternalId(long boardId, string externalId, long toLaneId, int position,
            string wipOverrideReason = DefaultOverrideReason)
		{
			var body = new { Comment = (string.IsNullOrEmpty(wipOverrideReason) ? DefaultOverrideReason : wipOverrideReason) }.ToJSON();
			var resource = $"/Kanban/Api/Board/{boardId}/MoveCardByExternalId/{Uri.EscapeDataString(externalId)}/Lane/{toLaneId}/Position/{position}";
			return await _client.LegacyRequest<long>(resource, "post", body);
		}

		public async Task<CardAddResult> AddCard(long boardId, TransferObjects.Card newCard, string wipOverrideReason = DefaultOverrideReason)
		{
			var resource = $"/Kanban/Api/Board/{boardId}/AddCardWithWipOverride/Lane/{newCard.LaneId}/Position/{newCard.Index}";
			newCard.UserWipOverrideComment = (string.IsNullOrEmpty(wipOverrideReason) ? DefaultOverrideReason : wipOverrideReason);
			return await _client.LegacyRequest<CardAddResult>(resource, "post", newCard.ToJSON());
		}

		public async Task<IEnumerable<TransferObjects.Card>> AddCards(long boardId, IEnumerable<TransferObjects.Card> newCards, string wipOverrideReason = DefaultOverrideReason)
		{
			var resource = $"/Kanban/Api/Board/{boardId}/AddCards?wipOverrideComment={(string.IsNullOrEmpty(wipOverrideReason) ? DefaultOverrideReason : wipOverrideReason)}";
			return await _client.LegacyRequest<List<TransferObjects.Card>>(resource, "post", newCards.ToJSON());
		}

		public async Task<CardUpdateResult> UpdateCard(long boardId, TransferObjects.Card updatedCard, string wipOverrideReason = DefaultOverrideReason)
		{
			var resource = $"/Kanban/Api/Board/{boardId}/UpdateCardWithWipOverride";
			updatedCard.UserWipOverrideComment = wipOverrideReason;
			return await _client.LegacyRequest<CardUpdateResult>(resource, "post", updatedCard.ToJSON());
		}

		public async Task<CardUpdateResult> UpdateCard(IDictionary<string, object> cardFieldUpdates)
		{
			const string resource = "/Kanban/Api/Card/Update";
			return await _client.LegacyRequest<CardUpdateResult>(resource, "post", cardFieldUpdates.ToJSON());
		}

		public async Task<CardsUpdateResult> UpdateCards(long boardId, IEnumerable<TransferObjects.Card> updatedCards, string wipOverrideReason = DefaultOverrideReason)
		{
			var resource = $"/Kanban/Api/Board/{boardId}/UpdateCards?wipOverrideComment={(string.IsNullOrEmpty(wipOverrideReason) ? DefaultOverrideReason : wipOverrideReason)}";
			return await _client.LegacyRequest<CardsUpdateResult>(resource, "post", updatedCards.ToJSON());
		}

		public async Task<long> DeleteCard(long boardId, long cardId)
		{
			var resource = $"/Kanban/Api/Board/{boardId}/DeleteCard/{cardId}";
            return await _client.LegacyRequest<long>(resource, "post");
		}

		public async Task<CardsDeleteResult> DeleteCards(long boardId, IEnumerable<long> cardIds)
		{
			var resource = $"/Kanban/Api/Board/{boardId}/DeleteCards";
			return await _client.LegacyRequest<CardsDeleteResult>(resource, "post", cardIds.ToJSON());
		}

		public async Task<CardDelegationResult> DelegateCard(long cardId, long delegationBoardId)
		{
			var resource = $"Kanban/API/Card/Delegate/{cardId}/{delegationBoardId}";
			return await _client.LegacyRequest<CardDelegationResult>(resource, "post");
		}

		public async Task<IEnumerable<TransferObjects.Comment>> GetComments(long boardId, long cardId)
		{
			var resource = $"/Kanban/Api/Card/GetComments/{boardId}/{cardId}";
			return await _client.LegacyRequest<List<TransferObjects.Comment>>(resource);
		}

		public async Task<long> PostComment(long boardId, long cardId, TransferObjects.Comment comment)
		{
			var resource = $"/Kanban/Api/Card/SaveComment/{boardId}/{cardId}";
			return await _client.LegacyRequest<long>(resource, "post", comment.ToJSON());
		}

		public async Task<long> PostCommentByExternalId(long boardId, string externalId, TransferObjects.Comment comment)
		{
			var resource = $"/Kanban/Api/Card/SaveCommentByExternalId/{boardId}/{Uri.EscapeDataString(externalId)}";
			return await _client.LegacyRequest<long>(resource, "post", comment.ToJSON());
		}

		public async Task<IEnumerable<CardEvent>> GetCardHistory(long boardId, long cardId)
		{
			var resource = $"/Kanban/Api/Card/History/{boardId}/{cardId}";
			return await _client.LegacyRequest<List<CardEvent>>(resource);
		}

		public async Task<PaginationResult<CardView>> SearchCards(long boardId, SearchOptions options)
		{
			var resource = $"/Kanban/Api/Board/{boardId}/SearchCards";
            return await _client.LegacyRequest<PaginationResult<CardView>>(resource, "post", options.ToJSON());
		}

		public async Task<IEnumerable<CardList>> ListNewCards(long boardId)
		{
			var resource = $"/Kanban/Api/Board/{boardId}/ListNewCards";
			return await _client.LegacyRequest<List<CardList>>(resource);
		}

		public async Task<CardMoveBetweenBoardsResult> MoveCardToAnotherBoard(long cardId, long destBoardId)
		{
			var resource = $"/Kanban/Api/Card/MoveCardToAnotherBoard/{cardId}/{destBoardId}";
			return await _client.LegacyRequest<CardMoveBetweenBoardsResult>(resource, "post");
		}

		public async Task<DrillThroughStatistics> GetDrillThroughStatistics(long boardId, long cardId)
		{
			var resource = $"/Kanban/Api/Card/{boardId}/GetDrillThroughStatistics/{cardId}";
			return await _client.LegacyRequest<DrillThroughStatistics>(resource);
		}

		public async Task<Taskboard> GetTaskboardById(long boardId, long taskboardId)
		{
			var resource = $"/Kanban/API/Board/{boardId}/TaskBoard/{taskboardId}/Get/";
			return await _client.LegacyRequest<Taskboard>(resource);
		}

		public async Task<Taskboard> GetTaskboard(long boardId, long cardId)
		{
			var resource = $"/Kanban/API/v1/Board/{boardId}/Card/{cardId}/Taskboard";
			return await _client.LegacyRequest<Taskboard>(resource);
		}

		public async Task<CardAddResult> AddTask(long boardId, long cardId, TransferObjects.Card newTask, string wipOverrideReason = DefaultOverrideReason)
		{
			var resource = $"/Kanban/Api/v1/Board/{boardId}/Card/{cardId}/Tasks/Lane/{newTask.LaneId}/Position/{newTask.Index}";
			newTask.UserWipOverrideComment = wipOverrideReason;
			return await _client.LegacyRequest<CardAddResult>(resource, "post", newTask.ToJSON());
		}

		public async Task<CardUpdateResult> UpdateTask(long boardId, long cardId, TransferObjects.Card updatedTask, string wipOverrideReason = DefaultOverrideReason)
		{
			var resource = $"/Kanban/Api/v1/Board/{boardId}/Update/Card/{cardId}/Tasks/{updatedTask.Id}";
			updatedTask.UserWipOverrideComment = wipOverrideReason;
			return await _client.LegacyRequest<CardUpdateResult>(resource, "post", updatedTask.ToJSON());
		}

		public async Task<long> DeleteTask(long boardId, long cardId, long taskId)
		{
			var resource = $"/Kanban/Api/v1/Board/{boardId}/Delete/Card/{cardId}/Tasks/{taskId}";
			return await _client.LegacyRequest<long>(resource, "post");
		}

		public async Task<BoardUpdates> CheckCardForTaskUpdates(long boardId, long cardId, long boardVersion)
		{
			var resource = $"/Kanban/Api/v1/Board/{boardId}/Card/{cardId}/Tasks/BoardVersion/{boardVersion}";
			return await _client.LegacyRequest<BoardUpdates>(resource);
		}

		public async Task<CardMoveResult> MoveTask(long boardId, long cardId, long taskId, long toLaneId, int position,
            string wipOverrideReason = DefaultOverrideReason)
		{
			var resource = $"/Kanban/Api/v1/Board/{boardId}/Move/Card/{cardId}/Tasks/{taskId}/Lane/{toLaneId}/Position/{position}";
			var body = new { Comment = (string.IsNullOrEmpty(wipOverrideReason) ? DefaultOverrideReason : wipOverrideReason) }.ToJSON();
			return await _client.LegacyRequest<CardMoveResult>(resource, "post", body);
		}

        public async Task<long> SaveAttachment(long boardId, long cardId, string fileName, string description, byte[] fileBytes)
        {
            var resource = $"/kanban/api/card/SaveAttachment/{boardId}/{cardId}";
            var parameters = new Dictionary<string, string> { { "Description", description }, { "FileName", fileName }, { "Id", "0" } };
            return await _client.LegacyPostFile<long>(resource, parameters, fileName, fileBytes);
        }

        public async Task<AssetFile> DownloadAttachment(long boardId, long attachmentId)
        {
            var resource = string.Format("/Kanban/Api/Card/DownloadAttachment/{0}/{1}", boardId, attachmentId);
            return await _client.LegacyGetFile(resource);
        }

		public async Task<long> DeleteAttachment(long boardId, long cardId, long attachmentId)
		{
			var resource = $"/Kanban/Api/Card/DeleteAttachment/{boardId}/{cardId}/{attachmentId}";
			return await _client.LegacyRequest<long>(resource, "post");
		}

		public async Task<Asset> GetAttachment(long boardId, long cardId, long attachmentId)
		{
			var resource = $"/Kanban/Api/Card/GetAttachments/{boardId}/{cardId}/{attachmentId}";
			return await _client.LegacyRequest<Asset>(resource);
		}

		public async Task<IEnumerable<Asset>> GetAttachments(long boardId, long cardId)
		{
			var resource = $"/Kanban/Api/Card/GetAttachments/{boardId}/{cardId}";
			return await _client.LegacyRequest<List<Asset>>(resource);
		}

		public async Task<TransferObjects.User> GetCurrentUser(long boardId)
		{
			var resource = "/Api/User/GetCurrentUserSettings/" + boardId;
			return await _client.LegacyRequest<TransferObjects.User>(resource);
		}
	}
}