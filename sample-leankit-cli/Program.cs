using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LeanKit.API;
using LeanKit.Models;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

namespace leankit.cli
{
    class Program
    {
        static void Main()
        {
            if (File.Exists("./settings.json"))
            {
                var settingsText = File.ReadAllText("./settings.json");
                var config = JsonConvert.DeserializeObject<Config>(settingsText);
                var task = RunTheTests(config);
                task.Wait();
                Console.WriteLine("finished.");
            }
            else
            {
                Console.WriteLine("Error: settings.json file not found");
            }
        }

        static async Task<bool> RunTheTests(Config config)
        {
            try
            {
                var client = string.IsNullOrEmpty(config.Token)
                           ? new Client(config.HostName, config.UserName, config.Password)
                           : new Client(config.HostName, config.Token);

                //await TestAccount(client);
                //await TestAuth(client);
                //await TestBoards(client);
                //await TestBoardCustomFields(client, 212159219);
                //await TestCards(client);
                //await TestTemplate(client);
                //await TestUser(client);
                //await TestTaskCards(client, config.TestBoardId, config.TestCardId);
                //await TestLegacyBoards(client, config.TestBoardId);
				await TestLegacyCards(client, config.TestBoardId);
				return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        static async Task TestLegacyCards(Client client, long boardId)
        {
            Console.WriteLine("Get Board Identifiers...");
            var boardIdentifiers = await client.Legacy.GetBoardIdentifiers(boardId);
            var cardType = boardIdentifiers.CardTypes[0];

            var now = DateTime.Now.ToString("yyyyMMddhhmmss");
            var externalId = $"TC-{now}";
            Console.WriteLine("Add a card...");
            var addCard = await client.Legacy.AddCard(boardId, new LeanKit.API.Legacy.Library.TransferObjects.Card
            {
                ExternalCardID = externalId,
                Tags = "tag1,tag2",
                Size = 5,
                Priority = 2,
                Index = 999,
                TypeId = cardType.Id,
                Title = "Test Card " + now,
                Description = "Card created by the legacy API"
            });
            Console.WriteLine(addCard.ToJSON());
            var cardId = addCard.CardId;

			Console.WriteLine("Get a card...");
			var card = await client.Legacy.GetCard(boardId, cardId);
			Console.WriteLine(card.ToJSON());

			Console.WriteLine("Get a card by external id...");
			var cardByExternalId = await client.Legacy.GetCardByExternalId(boardId, externalId);
			Console.WriteLine(cardByExternalId.ToJSON());

            Console.WriteLine("Add a comment...");
            var addComment = await client.Legacy.PostComment(boardId, cardId, new LeanKit.API.Legacy.Library.TransferObjects.Comment
            {
                Text = "Adding a test comment"
            });
			Console.WriteLine(addComment.ToJSON());

			var comments = await client.Legacy.GetComments(boardId, cardId);
            Console.WriteLine(comments.ToJSON());

			Console.WriteLine("adding an attachment...");
			var saveAttachment = await client.Legacy.SaveAttachment(boardId, cardId, "forward-back-slash.jpg", "how to remember back and forward slash", File.ReadAllBytes("./forward-back-slash.jpg"));
			Console.WriteLine(saveAttachment.ToJSON());

			Console.WriteLine("getting a list of attachments...");
            var attachments = await client.Legacy.GetAttachments(boardId, cardId);
			Console.WriteLine(attachments.ToJSON());
            var attachment = attachments.First();

			Console.WriteLine("downloading an attachment...");
            var assetFile = await client.Legacy.DownloadAttachment(boardId, attachment.Id);
			File.WriteAllBytes("./test.jpg", assetFile.FileBytes);
			Console.WriteLine(new { assetFile.ContentType, assetFile.ContentLength }.ToJSON());

			Console.WriteLine("deleting attachment...");
            await client.Legacy.DeleteAttachment(boardId, cardId, attachment.Id);

			Console.WriteLine("Delete card...");
			var boardVersion = await client.Legacy.DeleteCard(boardId, cardId);
			Console.WriteLine(boardVersion);

		}

		static async Task TestLegacyBoards(Client client, long boardId) 
        {
            Console.WriteLine("Get Boards...");
            var boards = await client.Legacy.GetBoards();
			Console.WriteLine(boards.ToJSON());

			Console.WriteLine("Get New Boards...");
			var newBoards = await client.Legacy.ListNewBoards();
			Console.WriteLine(newBoards.ToJSON());

            Console.WriteLine("Get a Board...");
            var board = await client.Legacy.GetBoard(boardId);
            Console.WriteLine(board.ToJSON());

			Console.WriteLine("Get Backlog Lanes...");
			var backlogLanes = await client.Legacy.GetBacklogLanes(boardId);
			Console.WriteLine(backlogLanes.ToJSON());

            Console.WriteLine("Get Archive Lanes...");
			var archiveLanes = await client.Legacy.GetArchiveLanes(boardId);
			Console.WriteLine(archiveLanes.ToJSON());

            Console.WriteLine("Get Archive Cards...");
			var archiveCards = await client.Legacy.GetArchiveCards(boardId);
			Console.WriteLine(archiveCards.ToJSON());

			Console.WriteLine("Get Newer Board If Exists...");
			var newerIfExists = await client.Legacy.GetNewerIfExists(boardId, board.Version - 1);
			Console.WriteLine(newerIfExists.ToJSON());

			Console.WriteLine("Get Board History Since...");
			var historySince = await client.Legacy.GetBoardHistorySince(boardId, board.Version - 1);
			Console.WriteLine(historySince.ToJSON());

			Console.WriteLine("Get Check for Updates...");
			var checkForUpdates = await client.Legacy.CheckForUpdates(boardId, board.Version - 1);
			Console.WriteLine(checkForUpdates.ToJSON());
		
            Console.WriteLine("Get Board Identifiers...");
			var boardIdentifiers = await client.Legacy.GetBoardIdentifiers(boardId);
			Console.WriteLine(boardIdentifiers.ToJSON());

		}

        static async Task TestTaskCards(Client client, long boardId, long cardId)
        {
            var board = await client.Board.Get(boardId);
            var taskType = board.CardTypes.FirstOrDefault(x => x.IsTaskType == true);
            if ( taskType == null ){
                throw new Exception("Test Board does not have any task card types");
            }

            Console.WriteLine("creating a task card...");
            var newTask = await client.TaskCard.Create(new TaskCardCreateRequest { 
                CardId = cardId,
                Title = "Test task card created with API",
                TypeId = taskType.Id,
                Size = 5,
                AssignedUserIds = new List<long> { board.Users[0].Id },
                BlockReason = "because I said so",
                PlannedStart = DateTime.Now.AddDays(5),
                PlannedFinish = DateTime.Now.AddDays(10),
                Description = "Description of this test card.",
                Priority = Priority.Critical,
                LaneType = LaneType.InProcess
            });
            Console.WriteLine(newTask.ToJSON());

			var card = await client.Card.Get(cardId);

			Console.WriteLine("getting task cards...");
			var taskList = await client.TaskCard.List(cardId);
			Console.WriteLine(taskList.ToJSON());
			
            Console.WriteLine("getting a the new task card...");
			var taskCard = await client.TaskCard.Get(cardId, newTask.Id);
            Console.WriteLine(taskCard.ToJSON());
		}

        static async Task TestUser(Client client)
        {
            Console.WriteLine("getting users...");
            var userList = await client.User.List(new UserListRequest{ Limit = 5 });
            Console.WriteLine(userList.ToJSON());

            var userId = userList.Users[0].Id;
            Console.WriteLine("get user: {0}", userId);
            var user = await client.User.Get(userId);
            Console.WriteLine(user.ToJSON());

			Console.WriteLine("getting me...");
            var me = await client.User.Me();
			Console.WriteLine(me.ToJSON());

            Console.WriteLine("getting recent boards...");
			var recentBoards = await client.User.Boards.Recent();
			Console.WriteLine(recentBoards.ToJSON());
		}

        static async Task TestTemplate(Client client)
        {

			Console.WriteLine("getting boards...");
			var boardList = await client.Board.List(new BoardListRequest { Limit = 5 });
			var boardId = boardList.Boards[0].Id;

            Console.WriteLine("creating template from board id: {0}", boardId);
            var templateCreateRes = await client.Template.Create(new TemplateCreateRequest 
            { 
                BoardId = boardId, 
                TemplateName = string.Format("Testing Template from {0}", boardId), 
                IncludeCards = false 
            });
            Console.WriteLine(templateCreateRes.ToJSON());
            var templateId = templateCreateRes.Id;

			Console.WriteLine("getting templates...");
            var templates = await client.Template.List();
			Console.WriteLine(templates.ToJSON());

            Console.WriteLine("deleting template...");
            await client.Template.Delete(templateId);
		}

        static async Task TestAccount(Client client)
        {
            Console.WriteLine("getting account info...");
            var account = await client.Account.Get();
            Console.WriteLine(account.ToJSON());
        }

        static async Task TestAuth(Client client)
        {
            Console.WriteLine("creating a token...");
            var token = await client.Auth.Token.Create("testing a token in dotnet");
            Console.WriteLine(token.ToJSON());

            Console.WriteLine("getting list of tokens...");
            var tokenList = await client.Auth.Token.List();
            Console.WriteLine(tokenList.ToJSON());

            Console.WriteLine("revoking the token...");
            await client.Auth.Token.Revoke(token.Id);
            Console.WriteLine("didn't blow up, so assumed revoked");

            Console.WriteLine("getting list of tokens...");
            var tokenList2 = await client.Auth.Token.List();
            Console.WriteLine(tokenList2.ToJSON());

        }

        static async Task TestBoardCustomFields(Client client, long boardId)
        {
            Console.WriteLine("getting a list of custom fields...");
            var fields = await client.Board.CustomField.List(boardId);
            Console.WriteLine(fields.ToJSON());

            Console.WriteLine("adding a custom field to the board...");
            var addRequest = new List<BoardCustomFieldUpdateOperation>
            {
                new BoardCustomFieldUpdateOperation
                {
                    Operation = BoardCustomFieldUpdateOperation.OperationEnum.Add,
                    Path = "/",
                    Value = new BoardCustomFieldUpdateOperation.UpdateOperationValue
                    {
                        Label = "Generic Link",
                        HelpText = "Just a link or something",
                        Type = BoardCustomFieldUpdateOperation.CustomFieldTypeEnum.Text
                    }
                }
            };
            var addedFields = await client.Board.CustomField.Update(boardId, addRequest);
            Console.WriteLine(addedFields.ToJSON());
            var customFieldId = addedFields.CustomFields[addedFields.CustomFields.Count - 1].Id;

            Console.WriteLine("updating a custom field...");
            var updateRequest = new List<BoardCustomFieldUpdateOperation>
            {
                new BoardCustomFieldUpdateOperation
                {
                    Operation = BoardCustomFieldUpdateOperation.OperationEnum.Replace,
                    Path = "/" + customFieldId.ToString(),
                    Value = new BoardCustomFieldUpdateOperation.UpdateOperationValue
                    {
                        Label = "Generic Link Updated",
                        HelpText = "Just a link or something again",
                        Type = BoardCustomFieldUpdateOperation.CustomFieldTypeEnum.Text
                    }
                }
            };
            var updatedFields = await client.Board.CustomField.Update(boardId, updateRequest);
            Console.WriteLine(updatedFields.ToJSON());

            Console.WriteLine("removing a custom field...");
            var deleteRequest = new List<BoardCustomFieldUpdateOperation>
            {
                new BoardCustomFieldUpdateOperation
                {
                    Operation = BoardCustomFieldUpdateOperation.OperationEnum.Remove,
                    Path = "/" + customFieldId.ToString()
                }
            };
            var fieldsWithRemoved = await client.Board.CustomField.Update(boardId, deleteRequest);
            Console.WriteLine(fieldsWithRemoved.ToJSON());
        }


        static async Task TestBoards(Client client)
        {
            Console.WriteLine("getting list of boards...");
            var boardList = await client.Board.List(new LeanKit.Models.BoardListRequest { Limit = 5 });
            Console.WriteLine(boardList.Boards.ToJSON());

            var boardId = boardList.Boards[0].Id;
            Console.WriteLine("getting board [{0}]...", boardId);
            var board = await client.Board.Get(boardId);
            Console.WriteLine(board.ToJSON());

            Console.WriteLine("creating a new board...");
            var createBoardRequest = new LeanKit.Models.BoardCreateRequest
            {
                FromBoardId = boardId,
                Title = "Test Board from new DotNet"
            };
            var createResponse = await client.Board.Create(createBoardRequest);
            Console.WriteLine(createResponse.ToJSON());
        }

        static async Task TestCards(Client client)
        {
			Console.WriteLine("getting a list of cards...");
			var cardList = await client.Card.List(new CardListRequest { Search = "Custom Fields" });
            Console.WriteLine(cardList.Cards.ToJSON());
            var c1 = cardList.Cards[0];

            Console.WriteLine("creating a card...");
            var createRequest = new CardCreateRequest
            {
                BoardId = c1.Board.Id,
                Title = "Test card created with dotnet client",
                TypeId = c1.Type.Id,
                Description = "Test card created with dotnet client. Isn't this cool?"
            };
            var cardCreateResponse = await client.Card.Create(createRequest);
            Console.WriteLine(cardCreateResponse.ToJSON());

            Console.WriteLine("Getting new card by id...");
            var card = await client.Card.Get(cardCreateResponse.Id);
            Console.WriteLine(card.ToJSON());

			Console.WriteLine("Updating new card...");
            var operations = new List<CardUpdateOperation>
            {
                new CardUpdateOperation
                {
                    Path = "/title",
                    Operation = CardUpdateOperation.OperationEnum.Replace,
                    Value = "Updated test card created with dotnet client"
                },
                new CardUpdateOperation
                {
                    Path = "/tags/-",
                    Operation = CardUpdateOperation.OperationEnum.Add,
                    Value = "web"
                }
            };
			var update = await client.Card.Update(card.Id, operations);
			Console.WriteLine(update.ToJSON());

			Console.WriteLine("Adding a new comment...");
            var commentResponse = await client.Card.Comment.Create(card.Id, "Testing a comment");
			Console.WriteLine(commentResponse.ToJSON());

			Console.WriteLine("Updating a new comment...");
			var commentUpdateResponse = await client.Card.Comment.Update(card.Id, commentResponse.Id, "Updating a comment");
            Console.WriteLine((commentUpdateResponse.ToJSON()));

			Console.WriteLine("Getting a list of comments...");
			var commentListResponse = await client.Card.Comment.List(card.Id);
			Console.WriteLine(commentListResponse.ToJSON());

            Console.WriteLine("deleting a comment...");
			await client.Card.Comment.Delete(card.Id, commentResponse.Id);

            Console.WriteLine("adding an attachment...");
            var attachmentResponse = await client.Card.Attachment.Create(card.Id, "forward-back-slash.jpg", "", File.ReadAllBytes("./forward-back-slash.jpg"));
			Console.WriteLine(attachmentResponse.ToJSON());

			Console.WriteLine("getting a list of attachments...");
			var attachmentListResponse = await client.Card.Attachment.List(card.Id);
			Console.WriteLine(attachmentListResponse.ToJSON());

            Console.WriteLine("downloading an attachment...");
            var attachmentFileResponse = await client.Card.Attachment.Download(card.Id, attachmentResponse.Id);
            File.WriteAllBytes("./test.jpg", attachmentFileResponse.FileBytes);
            Console.WriteLine( new { attachmentFileResponse.ContentType, attachmentFileResponse.ContentLength }.ToJSON());

			Console.WriteLine("deleting attachment...");
			await client.Card.Attachment.Delete(card.Id, attachmentResponse.Id);

			Console.WriteLine("deleting card...");
            await client.Card.Delete(card.Id);

		}

        static async Task RunTheCardExample(Config config)
        {
            var cardId = 522529728;
            var example = new LeanKitCardExample(config.HostName, config.UserName, config.Password);
            var card = await example.GetCard(cardId);
            Console.WriteLine(card.ToJSON());

            var field = card.CustomFields[0];
            var cardUpdate = await example.UpdateCustomField(cardId, field.FieldId, 0, field.Value + "-updated");
            Console.WriteLine(cardUpdate.ToJSON());
        }

    }

    public class Config
    {
        public string HostName { get; set; }
        public string Token { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public long TestBoardId { get; set; }
        public long TestCardId { get; set; }
    }
}
