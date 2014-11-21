//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using LeanKit.API.Client.Library;
using LeanKit.API.Client.Library.TransferObjects;
using LeanKitCmdQuery.Mappings;
using PowerArgs;
using ServiceStack.Text;

namespace LeanKitCmdQuery
{
	public class LeanKitQuery
	{
		private readonly QueryArgs _args;

		public LeanKitQuery(QueryArgs queryArgs)
		{
			_args = queryArgs;
		}

		private void WriteInfo(string output)
		{
			if (_args.Csv || _args.Json) return;
			Console.WriteLine(output);
		}

		private void ValidateQuery()
		{
			if (_args.Boards) return;
			if (_args.Board > 0) return;
			throw new ArgException("At least one query parameter is required.");
		}

		public void RunQuery()
		{
			ValidateQuery();

			var leanKitAuth = new LeanKitBasicAuth
			{
				Hostname = _args.Host,
				Username = _args.User,
				Password = _args.Password
			};

			// For internal development testing
			if (_args.Host.Equals("kanban-cibuild", StringComparison.InvariantCultureIgnoreCase)) 
				leanKitAuth.UrlTemplateOverride = "http://{0}.localkanban.com";

			WriteInfo("Connecting to LeanKit account...");
			var api = new LeanKitClientFactory().Create(leanKitAuth);

			if (_args.Boards)
			{
				WriteInfo("Getting all boards...");
				var boards = api.GetBoards();
				var boardList = boards.Select(Mapper.Map<BoardLiteView>);
				var output = _args.Csv ? boardList.ToCsv() : _args.Json ? boardList.ToJson() : boardList.Dump();
				Console.WriteLine(output);
			}
			else if (_args.Board > 0)
			{
				WriteInfo(string.Format("Getting board [{0}]...", _args.Board));
				var board = api.GetBoard(_args.Board);
				if (_args.Lanes || _args.Lane > 0 || _args.Cards)
				{
					// Get lanes
					var boardLanes = new List<Lane>();
					if (_args.Lane > 0)
					{
						WriteInfo(string.Format("Getting lane [{0}]...", _args.Lane));
						boardLanes.AddRange(board.AllLanes().Where(lane => lane.Id == _args.Lane));
					}
					else
					{
						if (_args.IncludeBacklog) boardLanes.AddRange(board.Backlog);
						boardLanes.AddRange(board.Lanes);
						if (_args.IncludeArchive) boardLanes.AddRange(board.Archive);
					}

					if (_args.Cards)
					{
						WriteInfo("Getting cards...");
						var cards = new List<MappedCardView>();
						foreach (var lane in boardLanes)
						{
							cards.AddRange(lane.Cards.Select(Mapper.Map<MappedCardView>));
						}
						// Archived cards is a separate API call
						if (_args.IncludeArchive)
						{
							var archivedCards = api.GetArchiveCards(_args.Board);
							cards.AddRange(archivedCards.Select(Mapper.Map<MappedCardView>));
						}
						var output = _args.Csv ? cards.ToCsv() : _args.Json ? cards.ToJson() : cards.Dump();
						Console.WriteLine(output);
					}
					else
					{
						var lanes = boardLanes.Select(Mapper.Map<LaneView>);
						var output = _args.Csv ? lanes.ToCsv() : _args.Json ? lanes.ToJson() : lanes.Dump();
						Console.WriteLine(output);
					}
				}
				else
				{
					var boardView = Mapper.Map<BoardView>(board);
					var output = _args.Csv ? boardView.ToCsv() : _args.Json ? boardView.ToJson() : boardView.Dump();
					Console.WriteLine(output);
				}
			}
		}
	}
}
