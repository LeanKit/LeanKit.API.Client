//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

// Modify CardModel and CardViewToCardModel to export more or less fields 

using LeanKit.API.Client.Library.TransferObjects;

public class CardModel {
		public long CardId { get; set; }
		public long LaneId { get; set; }
		public string LaneTitle { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string Type { get; set; }
		public string Priority { get; set; }
		public int Size { get; set; }
}

public CardModel CardViewToCardModel(Lane lane, CardView card) {
	var model = new CardModel {
		CardId = card.Id,
		LaneId = card.LaneId,
		LaneTitle = lane.Title.Replace("\"", "\"\""),
		Title = card.Title.Replace("\"", "\"\""),
		Description = card.Description.Replace("\"", "\"\""),
		Type = card.TypeName,
		Priority = card.Priority.ToString(),
		Size = card.Size
	};
	return model;
}