//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LeanKit.API.Client.Library.Exceptions;
using NUnit.Framework;
using Rhino.Mocks;
using LeanKit.API.Client.Library.TransferObjects;
using LeanKit.API.Client.Library.EventArguments;

namespace LeanKit.API.Client.Library.Tests
{
	[TestFixture]
	public class LeanKitIntegrationTests
	{
		private LeanKitIntegration _integration;
		private ILeanKitApi _apiMock;

		[SetUp]
		public void Setup()
		{
			_apiMock = MockRepository.GenerateMock<ILeanKitApi>();
		}

		[TearDown]
		public void TearDown()
		{
		}

		[Test]
		public void WillMoveCardByEvent()
		{
			_apiMock.Expect(x => x.GetBoard(1)).Return(GetSampleBoard());
			_apiMock.Expect(x => x.CheckForUpdates(1, 0)).Return(new BoardUpdates
			{
				HasUpdates = true,
				AffectedLanes = new List<Lane>
				{
					new Lane
					{
						Id = 1,
						Title = "Lane 1",
						Cards = new List<CardView>
						{
							new CardView
							{
								Id = 2,
								Title = "Card 2",
								LaneId = 2,
								Description = "some desc 2"
							}
						},
					},
					new Lane
					{
						Id = 2,
						Title = "Lane 2",
						Cards = new List<CardView>
						{
							new CardView
							{
								Id = 1,
								Title = "Card 1",
								LaneId = 2,
								Description = "some desc 1"
							},
							new CardView
							{
								Id = 3,
								Title = "Card 3",
								LaneId = 2,
								Description = "some desc 3"
							}
						}
					}
				},
				CurrentBoardVersion = 2,
				Events = new List<BoardHistoryEvent>
				{
					new BoardHistoryEvent
					{
						CardId = 1,
						ToLaneId = 2,
						FromLaneId = 1,
						UserId = 1,
						EventType = "LeanKit.Core.Events.CardMoveEvent"
					}
				}
			});

			_integration = new LeanKitIntegration(1, _apiMock);
			_integration.ShouldContinue = false;
			_integration.BoardChanged += ((object sender, BoardChangedEventArgs eventArgs) =>
			{
				Assert.NotNull(eventArgs);
				Assert.AreEqual(1, eventArgs.MovedCards.Count);
			});

			_integration.StartWatching();
			var affectedBoard = _integration.GetBoard();
			Assert.Null(affectedBoard.GetLaneById(1).Cards.FirstOrDefault(x => x.Id == 1));
			Assert.NotNull(affectedBoard.GetLaneById(2).Cards.First(x => x.Id == 1));
		}

		[Test]
		public void WillMoveCardWithWipOverride()
		{
			Board board = GetSampleBoard();
			board.Lanes[1].CardLimit = 1;
			_apiMock.Expect(x => x.GetBoard(1)).Return(board);

			_apiMock.Expect(x => x.CheckForUpdates(1, 0)).Return(new BoardUpdates
			{
				HasUpdates = true,
				AffectedLanes = new List<Lane>
				{
					new Lane
					{
						Id = 1,
						Title = "Lane 1",
						Cards = new List<CardView>
						{
							new CardView
							{
								Id = 2,
								Title = "Card 2",
								LaneId = 2,
								Description = "some desc 2"
							}
						},
					},
					new Lane
					{
						Id = 2,
						Title = "Lane 2",
						Cards = new List<CardView>
						{
							new CardView
							{
								Id = 1,
								Title = "Card 1",
								LaneId = 2,
								Description = "some desc 1"
							},
							new CardView
							{
								Id = 3,
								Title = "Card 3",
								LaneId = 2,
								Description = "some desc 3"
							}
						}
					}
				},
				CurrentBoardVersion = 3,
				Events = new List<BoardHistoryEvent>
				{
					new BoardHistoryEvent
					{
						CardId = 1,
						ToLaneId = 2,
						FromLaneId = 1,
						UserId = 1,
						EventType = "LeanKit.Core.Events.CardMoveEvent"
					},
					new BoardHistoryEvent
					{
						CardId = 3,
						ToLaneId = 2,
						FromLaneId = 1,
						UserId = 1,
						EventType = "LeanKit.Core.Events.CardMoveEvent"
					},
					new BoardHistoryEvent
					{
						WipOverrideComment = "some override",
						WipOverrideLane = 2,
						CardId = 3,
						ToLaneId = 2,
						FromLaneId = 1,
						UserId = 1,
						EventType = "LeanKit.Core.Events.WipOverrideEvent"
					}
				}
			});

			_integration = new LeanKitIntegration(1, _apiMock);
			_integration.ShouldContinue = false;
			_integration.BoardChanged += ((object sender, BoardChangedEventArgs eventArgs) =>
			{
				Assert.NotNull(eventArgs);
				Assert.AreEqual(2, eventArgs.MovedCards.Count);
				Assert.AreEqual(1, eventArgs.WipOverrides.Count);
			});
			_integration.StartWatching();

			var affectedBoard = _integration.GetBoard();
			Assert.IsNull(affectedBoard.GetLaneById(1).Cards.FirstOrDefault(x => x.Id == 1));
			Assert.IsNull(affectedBoard.GetLaneById(1).Cards.FirstOrDefault(x => x.Id == 3));
			Assert.NotNull(affectedBoard.GetLaneById(2).Cards.First(x => x.Id == 1));
			Assert.NotNull(affectedBoard.GetLaneById(2).Cards.First(x => x.Id == 3));
		}

		[Test]
		public void WillUpdateCardFields()
		{
			Board board = GetSampleBoard();
			_apiMock.Expect(x => x.GetBoard(1)).Return(board);
			_apiMock.Expect(x => x.CheckForUpdates(1, 0)).Return(new BoardUpdates
			{
				HasUpdates = true,
				AffectedLanes = new List<Lane>
				{
					new Lane
					{
						Id = 1,
						Title = "Lane 1",
						Cards = new List<CardView>
						{
							new CardView
							{
								Id = 1,
								Title = "Card 1 Updated",
								LaneId = 1,
								Description = "different description 1",
								Type =
									new CardType
									{
										Id = 1,
										Name = "Card type 1",
										IconPath = @"C:\"
									}
							},
							new CardView
							{
								Id = 2,
								Title = "Card 2 Updated",
								LaneId = 2,
								Description = "some desc 2",
								Type =
									new CardType
									{
										Id = 1,
										Name = "Card type 1",
										IconPath = @"C:\"
									}
							}
						},
					}
				},
				CurrentBoardVersion = 3,
				Events = new List<BoardHistoryEvent>
				{
					new BoardHistoryEvent
					{
						CardId = 1,
						ToLaneId = 1,
						UserId = 1,
						EventType = "LeanKit.Core.Events.CardFieldsChangedEvent"
					},
				}
			});

			_integration = new LeanKitIntegration(1, _apiMock);
			_integration.ShouldContinue = false;
			_integration.BoardChanged += ((object sender, BoardChangedEventArgs eventArgs) =>
			{
				Assert.NotNull(eventArgs);
				Assert.AreEqual(1, eventArgs.UpdatedCards.Count);
				Assert.AreEqual(0, eventArgs.MovedCards.Count);
			});
			_integration.StartWatching();
			var affectedBoard = _integration.GetBoard();
			Assert.NotNull(affectedBoard.GetLaneById(1).Cards.First(x => x.Id == 1));
			Assert.NotNull(affectedBoard.GetLaneById(1).Cards.First(x => x.Id == 2));
			Assert.IsTrue(affectedBoard.GetLaneById(1).Cards.First(x => x.Id == 1).Title.Contains("Updated"));
			Assert.IsTrue(affectedBoard.GetLaneById(1).Cards.First(x => x.Id == 1).Description.Contains("different"));
			Assert.IsTrue(affectedBoard.GetLaneById(1).Cards.First(x => x.Id == 2).Title.Contains("Updated"));
		}

		[Test]
		public void WillBlockCard()
		{
			Board board = GetSampleBoard();
			_apiMock.Expect(x => x.GetBoard(1)).Return(board);
			_apiMock.Expect(x => x.CheckForUpdates(1, 0)).Return(new BoardUpdates
			{
				HasUpdates = true,
				AffectedLanes = new List<Lane>
				{
					new Lane
					{
						Id = 1,
						Title = "Lane 1",
						Cards = new List<CardView>
						{
							new CardView
							{
								Id = 1,
								IsBlocked = true,
								BlockReason = "comment",
								Title = "Card 1",
								LaneId = 1,
								Description = "some desc 1",
								Type =
									new CardType
									{
										Id = 1,
										Name = "Card type 1",
										IconPath = @"C:\"
									}
							},
							new CardView
							{
								Id = 2,
								Title = "Card 2",
								LaneId = 2,
								Description = "some desc 2",
								Type =
									new CardType
									{
										Id = 1,
										Name = "Card type 1",
										IconPath = @"C:\"
									}
							}
						},
					}
				},
				CurrentBoardVersion = 3,
				Events = new List<BoardHistoryEvent>
				{
					new BoardHistoryEvent
					{
						CardId = 1,
						ToLaneId = 1,
						BlockedComment = "comment",
						IsBlocked = true,
						UserId = 1,
						EventType = "LeanKit.Core.Events.CardBlockedEvent"
					},
				}
			});

			_integration = new LeanKitIntegration(1, _apiMock);
			_integration.ShouldContinue = false;
			_integration.BoardChanged += ((object sender, BoardChangedEventArgs eventArgs) =>
			{
				Assert.NotNull(eventArgs);
				Assert.AreEqual(1, eventArgs.BlockedCards.Count);
				var affectedBoard = _integration.GetBoard();
				Assert.NotNull(affectedBoard.GetLaneById(1).Cards.First(x => x.Id == 1));
				Assert.IsTrue(affectedBoard.GetLaneById(1).Cards.First(x => x.Id == 1).IsBlocked);
				Assert.IsNotNullOrEmpty(affectedBoard.GetLaneById(1).Cards.First(x => x.Id == 1).BlockReason);
			});
			_integration.StartWatching();
//			Thread.Sleep(1500);
		}

		[Test]
		public void WillUnBlockCard()
		{
			Board board = GetSampleBoard();
			board.Lanes.First(x => x.Id == 1).Cards.First(x => x.Id == 1).IsBlocked = true;

			_apiMock.Expect(x => x.GetBoard(1)).Return(board);
			_apiMock.Expect(x => x.CheckForUpdates(1, 0)).Return(new BoardUpdates
			{
				HasUpdates = true,
				AffectedLanes = new List<Lane>
				{
					new Lane
					{
						Id = 1,
						Title = "Lane 1",
						Cards = new List<CardView>
						{
							new CardView
							{
								Id = 1,
								IsBlocked = false,
								BlockReason = "no reason",
								Title = "Card 1",
								LaneId = 1,
								Description = "some desc 1",
								Type =
									new CardType
									{
										Id = 1,
										Name = "Card type 1",
										IconPath = @"C:\"
									}
							},
							new CardView
							{
								Id = 2,
								Title = "Card 2",
								LaneId = 2,
								Description = "some desc 2",
								Type =
									new CardType
									{
										Id = 1,
										Name = "Card type 1",
										IconPath = @"C:\"
									}
							}
						},
					}
				},
				CurrentBoardVersion = 3,
				Events = new List<BoardHistoryEvent>
				{
					new BoardHistoryEvent
					{
						CardId = 1,
						ToLaneId = 1,
						BlockedComment = "unblock",
						IsBlocked = false,
						UserId = 1,
						EventType = "LeanKit.Core.Events.CardBlockedEvent"
					},
				}
			});

			_integration = new LeanKitIntegration(1, _apiMock);
			_integration.ShouldContinue = false;
			_integration.BoardChanged += ((object sender, BoardChangedEventArgs eventArgs) =>
			{
				Assert.NotNull(eventArgs);
				Assert.AreEqual(1, eventArgs.UnBlockedCards.Count);
			});
			_integration.StartWatching();
			var affectedBoard = _integration.GetBoard();
			Assert.NotNull(affectedBoard.GetLaneById(1).Cards.FirstOrDefault(x => x.Id == 1));
			Assert.IsFalse(affectedBoard.GetLaneById(1).Cards.First(x => x.Id == 1).IsBlocked);
		}

		[Test]
		public void WillDeleteCards()
		{
			Board board = GetSampleBoard();
			_apiMock.Expect(x => x.GetBoard(1)).Return(board);
			_apiMock.Expect(x => x.CheckForUpdates(1, 0)).Return(new BoardUpdates
			{
				HasUpdates = true,
				AffectedLanes = new List<Lane>
				{
					new Lane
					{
						Id = 1,
						Title = "Lane 1",
						Cards = new List<CardView>
						{
							new CardView
							{
								Id = 2,
								Title = "Card 2",
								LaneId = 2,
								Description = "some desc 2"
							}
						},
					}
				},
				CurrentBoardVersion = 3,
				Events = new List<BoardHistoryEvent>
				{
					new BoardHistoryEvent
					{
						CardId = 1,
						ToLaneId = 1,
						UserId = 1,
						EventType = "LeanKit.Core.Events.CardDeletedEvent"
					},
				}
			});

			_integration = new LeanKitIntegration(1, _apiMock);
			_integration.ShouldContinue = false;
			_integration.BoardChanged += ((object sender, BoardChangedEventArgs eventArgs) =>
			{
				Assert.NotNull(eventArgs);
				Assert.AreEqual(1, eventArgs.DeletedCards.Count);
				Assert.AreEqual(0, eventArgs.MovedCards.Count);
				var affectedBoard = _integration.GetBoard();
				Assert.IsNull(affectedBoard.GetLaneById(1).Cards.FirstOrDefault(x => x.Id == 1));
			});
			_integration.StartWatching();
		}

		[Test]
		public void WillCreateCard()
		{
			Board board = GetSampleBoard();
			_apiMock.Expect(x => x.GetBoard(1)).Return(board);
			_apiMock.Expect(x => x.CheckForUpdates(1, 0)).Return(new BoardUpdates
			{
				HasUpdates = true,
				AffectedLanes = new List<Lane>
				{
					new Lane
					{
						Id = 1,
						Title = "Lane 1",
						Cards = new List<CardView>
						{
							new CardView
							{
								Id = 1,
								Title = "Card 1",
								LaneId = 1,
								Description = "some desc 1",
								Type =
									new CardType
									{
										Id = 1,
										Name = "Card type 1",
										IconPath = @"C:\"
									}
							},
							new CardView
							{
								Id = 2,
								Title = "Card 2",
								LaneId = 1,
								Description = "some desc 2",
								Type =
									new CardType
									{
										Id = 1,
										Name = "Card type 1",
										IconPath = @"C:\"
									}
							},
							new CardView
							{
								Id = 4,
								Title = "Card 4",
								LaneId = 1,
								Description = "some desc 4",
								Type =
									new CardType
									{
										Id = 1,
										Name = "Card type 1",
										IconPath = @"C:\"
									}
							}
						},
					}
				},
				CurrentBoardVersion = 3,
				Events = new List<BoardHistoryEvent>
				{
					new BoardHistoryEvent
					{
						CardId = 4,
						ToLaneId = 1,
						UserId = 1,
						EventType = "LeanKit.Core.Events.CardCreationEvent"
					},
				}
			});

			_integration = new LeanKitIntegration(1, _apiMock);
			_integration.ShouldContinue = false;
			_integration.BoardChanged += ((object sender, BoardChangedEventArgs eventArgs) =>
			{
				Assert.NotNull(eventArgs);
				Assert.AreEqual(1, eventArgs.AddedCards.Count);
				Assert.AreEqual(0, eventArgs.MovedCards.Count);
				var affectedBoard = _integration.GetBoard();
				Assert.NotNull(affectedBoard.GetLaneById(1).Cards.First(x => x.Id == 4));
			});
			_integration.StartWatching();
		}

		[Test]
		public void WillAssignUserToCard()
		{
			Board board = GetSampleBoard();
			_apiMock.Expect(x => x.GetBoard(1)).Return(board);
			_apiMock.Expect(x => x.CheckForUpdates(1, 0)).Return(new BoardUpdates
			{
				HasUpdates = true,
				AffectedLanes = new List<Lane>
				{
					new Lane
					{
						Id = 1,
						Title = "Lane 1",
						Cards = new List<CardView>
						{
							new CardView
							{
								Id = 1,
								AssignedUsers =
									new List<AssignedUserInfo>
									{
										new AssignedUserInfo
										{
											AssignedUserId = 1,
											AssignedUserName = "User 1",
											GravatarLink = "",
											SmallGravatarLink = ""
										}
									},
								Title = "Card 1",
								LaneId = 1,
								Description = "some desc 1",
								Type =
									new CardType
									{
										Id = 1,
										Name = "Card type 1",
										IconPath = @"C:\"
									}
							},
							new CardView
							{
								Id = 2,
								Title = "Card 2",
								LaneId = 1,
								Description = "some desc 2",
								Type =
									new CardType
									{
										Id = 1,
										Name = "Card type 1",
										IconPath = @"C:\"
									}
							},
						},
					}
				},
				CurrentBoardVersion = 3,
				Events = new List<BoardHistoryEvent>
				{
					new BoardHistoryEvent
					{
						CardId = 1,
						ToLaneId = 1,
						AssignedUserId = 1,
						IsUnassigning = false,
						UserId = 1,
						EventType = "LeanKit.Core.Events.UserAssignmentEvent"
					},
				}
			});

			_integration = new LeanKitIntegration(1, _apiMock);
			_integration.ShouldContinue = false;
			_integration.BoardChanged += ((object sender, BoardChangedEventArgs eventArgs) =>
			{
				Assert.NotNull(eventArgs);
				Assert.AreEqual(1, eventArgs.AssignedUsers.Count);
				
				var affectedBoard = _integration.GetBoard();
				Assert.NotNull(affectedBoard.GetLaneById(1).Cards.First(x => x.Id == 1));
				Assert.IsNotEmpty(affectedBoard.GetLaneById(1).Cards.First(x => x.Id == 1).AssignedUsers);
				Assert.IsNotNull(
					affectedBoard.GetLaneById(1).Cards.First(x => x.Id == 1).AssignedUsers.FirstOrDefault(x => x.AssignedUserId == 1));
			});
			_integration.StartWatching();
		}

		[Test]
		public void WillUnAssignUserFromCard()
		{
			Board board = GetSampleBoard();

			board.Lanes[0].Cards[0].AssignedUsers = new List<AssignedUserInfo>
			{new AssignedUserInfo {AssignedUserId = 1, AssignedUserName = "User 1", GravatarLink = "", SmallGravatarLink = ""}};

			_apiMock.Expect(x => x.GetBoard(1)).Return(board);
			_apiMock.Expect(x => x.CheckForUpdates(1, 0)).Return(new BoardUpdates
			{
				HasUpdates = true,
				AffectedLanes = new List<Lane>
				{
					new Lane
					{
						Id = 1,
						Title = "Lane 1",
						Cards = new List<CardView>
						{
							new CardView
							{
								Id = 1,
								Title = "Card 1",
								LaneId = 1,
								Description = "some desc 1",
								Type =
									new CardType
									{
										Id = 1,
										Name = "Card type 1",
										IconPath = @"C:\"
									}
							},
							new CardView
							{
								Id = 2,
								Title = "Card 2",
								LaneId = 1,
								Description = "some desc 2",
								Type =
									new CardType
									{
										Id = 1,
										Name = "Card type 1",
										IconPath = @"C:\"
									}
							},
						},
					}
				},
				CurrentBoardVersion = 3,
				Events = new List<BoardHistoryEvent>
				{
					new BoardHistoryEvent
					{
						CardId = 1,
						ToLaneId = 1,
						AssignedUserId = 1,
						IsUnassigning = true,
						UserId = 1,
						EventType = "LeanKit.Core.Events.UserAssignmentEvent"
					},
				}
			});

			_integration = new LeanKitIntegration(1, _apiMock);
			_integration.ShouldContinue = false;
			_integration.BoardChanged += ((object sender, BoardChangedEventArgs eventArgs) =>
			{
				Assert.NotNull(eventArgs);
				Assert.AreEqual(1, eventArgs.UnAssignedUsers.Count);
			});

			_integration.StartWatching();
			var affectedBoard = _integration.GetBoard();
			Assert.NotNull(affectedBoard.GetLaneById(1).Cards.First(x => x.Id == 1));
			Assert.IsNull(affectedBoard.GetLaneById(1).Cards.First(x => x.Id == 1).AssignedUsers);
		}

		[Test]
		public void WillPostCommentToCard()
		{
			Board board = GetSampleBoard();

			_apiMock.Expect(x => x.GetBoard(1)).Return(board);
			_apiMock.Expect(x => x.CheckForUpdates(1, 0)).Return(new BoardUpdates
			{
				HasUpdates = true,
				AffectedLanes = new List<Lane>
				{
					new Lane
					{
						Id = 1,
						Title = "Lane 1",
						Cards = new List<CardView>
						{
							new CardView
							{
								Id = 1,
								Title = "Card 1",
								LaneId = 1,
								Comments =
									new List<Comment>
									{
										new Comment
										{
											Id = 1,
											PostDate =
												DateTime.Now.
													ToShortDateString(),
											PostedById = 1,
											Text = "Hello, All"
										}
									},
								Description = "some desc 1",
								Type =
									new CardType
									{
										Id = 1,
										Name = "Card type 1",
										IconPath = @"C:\"
									}
							},
							new CardView
							{
								Id = 2,
								Title = "Card 2",
								LaneId = 1,
								Description = "some desc 2",
								Type =
									new CardType
									{
										Id = 1,
										Name = "Card type 1",
										IconPath = @"C:\"
									}
							},
						},
					}
				},
				CurrentBoardVersion = 3,
				Events = new List<BoardHistoryEvent>
				{
					new BoardHistoryEvent
					{
						CardId = 1,
						ToLaneId = 1,
						UserId = 1,
						EventType = "LeanKit.Core.Events.CommentPostEvent"
					},
				}
			});

			_integration = new LeanKitIntegration(1, _apiMock);
			_integration.ShouldContinue = false;
			_integration.BoardChanged += ((object sender, BoardChangedEventArgs eventArgs) =>
			{
				Assert.NotNull(eventArgs);
				Assert.AreEqual(1, eventArgs.PostedComments.Count);
			});

			_integration.StartWatching();
			var affectedBoard = _integration.GetBoard();
			Assert.IsNotEmpty(affectedBoard.GetLaneById(1).Cards.First(x => x.Id == 1).Comments);
			Assert.IsNotNull(affectedBoard.GetLaneById(1).Cards.First(x => x.Id == 1).Comments.FirstOrDefault(x => x.Id == 1));
		}

		[Test]
		public void WillOverrideLaneWip()
		{
			Board board = GetSampleBoard();

			_apiMock.Expect(x => x.GetBoard(1)).Return(board);
			_apiMock.Expect(x => x.CheckForUpdates(1, 0)).Return(new BoardUpdates
			{
				HasUpdates = true,
				AffectedLanes = new List<Lane>
				{
					new Lane
					{
						Id = 1,
						Title = "Lane 1",
						Cards = new List<CardView>
						{
							new CardView
							{
								Id = 1,
								Title = "Card 1",
								LaneId = 1,
								Comments =
									new List<Comment>
									{
										new Comment
										{
											Id = 1,
											PostDate =
												DateTime.Now.
													ToShortDateString(),
											PostedById = 1,
											Text = "Hello, All"
										}
									},
								Description = "some desc 1",
								Type =
									new CardType
									{
										Id = 1,
										Name = "Card type 1",
										IconPath = @"C:\"
									}
							},
							new CardView
							{
								Id = 2,
								Title = "Card 2",
								LaneId = 1,
								Description = "some desc 2",
								Type =
									new CardType
									{
										Id = 1,
										Name = "Card type 1",
										IconPath = @"C:\"
									}
							},
						},
					}
				},
				CurrentBoardVersion = 3,
				Events = new List<BoardHistoryEvent>
				{
					new BoardHistoryEvent
					{
						CardId = 1,
						WipOverrideLane = 1,
						UserId = 1,
						EventType = "LeanKit.Core.Events.WipOverrideEvent"
					},
				}
			});

			_integration = new LeanKitIntegration(1, _apiMock);
			_integration.ShouldContinue = false;
			_integration.BoardChanged += ((object sender, BoardChangedEventArgs eventArgs) =>
			{
				Assert.NotNull(eventArgs);
				Assert.AreEqual(1, eventArgs.WipOverrides.Count);
			});

			_integration.StartWatching();
		}

		[Test]
		public void WillOverrideUserWip()
		{
			Board board = GetSampleBoard();

			_apiMock.Expect(x => x.GetBoard(1)).Return(board);
			_apiMock.Expect(x => x.CheckForUpdates(1, 0)).Return(new BoardUpdates
			{
				HasUpdates = true,
				AffectedLanes = new List<Lane>
				{
					new Lane
					{
						Id = 1,
						Title = "Lane 1",
						Cards = new List<CardView>
						{
							new CardView
							{
								Id = 1,
								Title = "Card 1",
								LaneId = 1,
								Comments =
									new List<Comment>
									{
										new Comment
										{
											Id = 1,
											PostDate =
												DateTime.Now.
													ToShortDateString(),
											PostedById = 1,
											Text = "Hello, All"
										}
									},
								Description = "some desc 1",
								Type =
									new CardType
									{
										Id = 1,
										Name = "Card type 1",
										IconPath = @"C:\"
									}
							},
							new CardView
							{
								Id = 2,
								Title = "Card 2",
								LaneId = 1,
								Description = "some desc 2",
								Type =
									new CardType
									{
										Id = 1,
										Name = "Card type 1",
										IconPath = @"C:\"
									}
							},
						},
					}
				},
				CurrentBoardVersion = 3,
				Events = new List<BoardHistoryEvent>
				{
					new BoardHistoryEvent
					{
						CardId = 1,
						WipOverrideUser = 1,
						UserId = 1,
						EventType = "LeanKit.Core.Events.UserWipOverrideEvent"
					},
				}
			});

			_integration = new LeanKitIntegration(1, _apiMock);
			_integration.ShouldContinue = false;
			_integration.BoardChanged += ((object sender, BoardChangedEventArgs eventArgs) =>
			{
				Assert.NotNull(eventArgs);
				Assert.AreEqual(1, eventArgs.UserWipOverrides.Count);
			});

			_integration.StartWatching();
		}

		#region Call Through API tests

		[Test]
		public void CallWillGetCard()
		{
			Board board = GetSampleBoard();
			_apiMock.Expect(x => x.GetBoard(1)).Return(board);
			_integration = new LeanKitIntegration(1, _apiMock);
			_integration.ShouldContinue = false;
			_integration.StartWatching();
			Card card = _integration.GetCard(2);
			Assert.NotNull(card);
			Assert.AreEqual("Card 2", card.Title);
		}

		[Test]
		public void CallWillGetCardByExternalIdWithPrefix()
		{
			Board board = GetSampleBoard();
			_apiMock.Expect(x => x.GetBoard(1)).Return(board);
			_integration = new LeanKitIntegration(1, _apiMock);
			_integration.ShouldContinue = false;
			_integration.StartWatching();
			Card card = _integration.GetCardByExternalId(1, "CS124");
			Assert.NotNull(card);
			Assert.AreEqual("Card 2", card.Title);
		}

		[Test]
		public void CallWillGetCardByExternalIdWithoutPrefix()
		{
			Board board = GetSampleBoard();
			_apiMock.Expect(x => x.GetBoard(1)).Return(board);
			_integration = new LeanKitIntegration(1, _apiMock);
			_integration.ShouldContinue = false;
			_integration.StartWatching();
			Card card = _integration.GetCardByExternalId(1, "123");
			Assert.NotNull(card);
			Assert.AreEqual("Card 1", card.Title);
		}

		[Test]
		public void CallWillUpdateCard()
		{
			Board board = GetSampleBoard();
			_apiMock.Expect(x => x.GetBoard(1)).Return(board);
			Card cardToUpdate = new Card
			{
				Id = 1,
				Title = "Card 1 Updated",
				LaneId = 1,
				Description = "some desc 1",
				TypeId = 1,
				ExternalCardID = "123"
			};
			CardUpdateResult result = new CardUpdateResult {BoardVersion = 1, CardDTO = cardToUpdate.ToCardView()};

			_apiMock.Expect(x => x.UpdateCard(Arg<long>.Is.Anything, Arg<Card>.Is.Anything)).Return(result);

			_integration = new LeanKitIntegration(1, _apiMock);
			_integration.ShouldContinue = false;
			_integration.StartWatching();


			_integration.UpdateCard(cardToUpdate);
			Card card = _integration.GetCard(1);
			Assert.NotNull(card);
			Assert.AreEqual("Card 1 Updated", card.Title);
		}

		[Test]
		public void CallWillAddCard()
		{
			Board board = GetSampleBoard();
			_apiMock.Expect(x => x.GetBoard(1)).Return(board);
			Card cardToAdd = new Card
			{
				Id = 1,
				Title = "Card 1 Updated",
				LaneId = 1,
				Description = "some desc 1",
				TypeId = 1,
				ExternalCardID = "123"
			};
			Lane affectedLane = new Lane
			{
				Id = 1,
				Title = "Lane 1",
				Cards = new List<CardView>
				{
					new CardView
					{
						Id = 1,
						Title = "Card 1",
						LaneId = 1,
						Description = "some desc 1",
						Type = new CardType {Id = 1, Name = "Card type 1", IconPath = @"C:\"}
					},
					new CardView
					{
						Id = 2,
						Title = "Card 2",
						LaneId = 1,
						Description = "some desc 2",
						Type = new CardType {Id = 1, Name = "Card type 1", IconPath = @"C:\"}
					},
					new CardView
					{
						Id = 4,
						Title = "Card 4",
						LaneId = 1,
						Description = "some desc 4",
						Type = new CardType {Id = 1, Name = "Card type 1", IconPath = @"C:\"}
					},
				},
			};
			CardAddResult result = new CardAddResult {BoardVersion = 1, CardId = 4, Lane = affectedLane};

			_apiMock.Expect(x => x.AddCard(Arg<long>.Is.Anything, Arg<Card>.Is.Anything)).Return(result);

			_integration = new LeanKitIntegration(1, _apiMock);
			_integration.ShouldContinue = false;
			_integration.StartWatching();

			_integration.AddCard(cardToAdd);

			Card card = _integration.GetCard(4);
			Assert.NotNull(card);
			Assert.AreEqual("Card 4", card.Title);
		}

		[Test]
		public void CallWillMoveCard()
		{
			Board board = GetSampleBoard();
			_apiMock.Expect(x => x.GetBoard(1)).Return(board);
			_apiMock.Expect(
				x => x.MoveCard(Arg<long>.Is.Anything, Arg<long>.Is.Anything, Arg<long>.Is.Anything, Arg<int>.Is.Anything))
				.Return(1);

			_integration = new LeanKitIntegration(1, _apiMock);
			_integration.ShouldContinue = false;
			_integration.StartWatching();

			_integration.MoveCard(1, 2, 1);

			Card card = _integration.GetCard(1);
			Assert.NotNull(card);
			Assert.AreEqual(2, card.LaneId);
			Assert.AreEqual(1, board.GetLaneById(2).Cards[0].Id);
			Assert.IsNull(board.GetLaneById(1).Cards.FirstOrDefault(x => x.Id == 1));
		}

		[Test]
		[ExpectedException(typeof (ItemNotFoundException))]
		public void CallWillDeleteCard()
		{
			Board board = GetSampleBoard();
			_apiMock.Expect(x => x.GetBoard(1)).Return(board);
			_apiMock.Expect(x => x.DeleteCard(Arg<long>.Is.Anything, Arg<long>.Is.Anything)).Return(1);

			_integration = new LeanKitIntegration(1, _apiMock);
			_integration.ShouldContinue = false;
			_integration.StartWatching();

			_integration.DeleteCard(1);

			_integration.GetCard(1);
		}

		#endregion

		private Board GetSampleBoard()
		{
			return new Board
			{
				Id = 1,
				Title = "Sample board",
				IsPrefixEnabled = true,
				Prefix = "CS",
				Backlog = new List<Lane>(),
				Archive = new List<Lane>(),
				Lanes = new List<Lane>
				{
					new Lane
					{
						Id = 1,
						Title = "Lane 1",
						Cards = new List<CardView>
						{
							new CardView
							{
								Id = 1,
								Title = "Card 1",
								LaneId = 1,
								Description = "some desc 1",
								Type = new CardType {Id = 1, Name = "Card type 1", IconPath = @"C:\"},
								ExternalCardID = "123"
							},
							new CardView
							{
								Id = 2,
								Title = "Card 2",
								LaneId = 2,
								Description = "some desc 2",
								Type = new CardType {Id = 1, Name = "Card type 1", IconPath = @"C:\"},
								ExternalCardID = "124"
							}
						}
					},
					new Lane {Id = 2, Title = "Lane 2"},
					new Lane {Id = 3, Title = "Lane 3"}
				},
				CardTypes = new List<CardType>
				{
					new CardType {Id = 1, Name = "Card type 1", IconPath = @"C:\"},
					new CardType {Id = 2, Name = "Card type 2", IconPath = @"C:\"},
					new CardType {Id = 3, Name = "Card type 3", IconPath = @"C:\"},
					new CardType {Id = 4, Name = "Card type 4", IconPath = @"C:\"},
				},
				ClassesOfService = new List<ClassOfService>
				{
					new ClassOfService {Id = 1, Title = "Class of service 1", IconPath = @"d:\"},
					new ClassOfService {Id = 2, Title = "Class of service 2", IconPath = @"d:\"},
					new ClassOfService {Id = 3, Title = "Class of service 3", IconPath = @"d:\"},
					new ClassOfService {Id = 4, Title = "Class of service 4", IconPath = @"d:\"},
				},
				BoardUsers = new List<User>
				{
					new User {Id = 1, UserName = "User 1", EmailAddress = "user1@google.com"},
					new User {Id = 2, UserName = "User 2", EmailAddress = "user2@google.com"},
					new User {Id = 3, UserName = "User 3", EmailAddress = "user3@google.com"},
					new User {Id = 4, UserName = "User 4", EmailAddress = "user4@google.com"},
				}
			};
		}
	}
}