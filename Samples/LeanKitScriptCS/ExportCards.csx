#r "LeanKit.API.Client.Library.dll"
#load "CardModel.csx"

//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using LeanKit.API.Client.Library;
using LeanKit.API.Client.Library.TransferObjects;
using System.Collections.Generic;
using System.Linq;

var host = "accountname";          // Change to your account name, e.g. https://accountname.leankit.com
var userName = "user@email.com";   // Change to your account email address
var password = "p@ssword";         // Change to your account password
var boardId = 101;                 // Change to your board ID, e.g. https://accountname.leankit.com/Boards/View/101

var leanKitAuth = new LeanKitBasicAuth {
		Hostname = host,
		Username = userName,
		Password = password
	};

var api = new LeanKitClientFactory().Create(leanKitAuth);

var board = api.GetBoard(boardId);
var cards = new List<CardModel>();

// Get all cards in all lanes
var allLanes = board.AllLanes();
foreach(var lane in allLanes) {
	foreach(var card in lane.Cards) {
		var model = CardViewToCardModel(lane, card);
		cards.Add(model);
	}
}

// Get archived cards
var archivedCards = api.GetArchiveCards(boardId);
foreach (var card in archivedCards) {
	var lane = allLanes.FirstOrDefault(x => x.Id == card.LaneId);
    var model = CardViewToCardModel(lane, card);
	cards.Add(model);
}

// Update the following code to export more or less fields

var formatStr = "{0},\"{1}\",{2},\"{3}\",\"{4}\",{5},{6},{7}";

Console.WriteLine("LaneId,LaneTitle,CardId,Title,Description,Type,Priority,Size");

foreach(var card in cards) {
	Console.WriteLine(formatStr, card.LaneId, card.LaneTitle, card.CardId, card.Title, card.Description, card.Type, card.Priority, card.Size);
}