//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Linq;
using AutoMapper;
using LeanKit.API.Client.Library.TransferObjects;

namespace LeanKitCmdQuery.Mappings
{
	public class Mappings
	{
		public void Init()
		{
			Mapper.CreateMap<BoardListing, BoardLiteView>();

			Mapper.CreateMap<Board, BoardView>()
				.ForMember(m => m.Lanes, opt => opt.ResolveUsing(b => b.AllLanes().Count()))
				.ForMember(m => m.ClassesOfService, opt => opt.ResolveUsing(b =>
				{
					var classes = b.ClassesOfService.Aggregate("", (current, cos) => current + (cos.Title + ", "));
					if (classes.Length > 2) classes = classes.Substring(0, classes.Length - 2);
					return classes;
				}))
				.ForMember(m => m.CardTypes, opt => opt.ResolveUsing(b =>
				{
					var cardTypes = b.CardTypes.Aggregate("", (current, cardType) => current + (cardType.Name + ", "));
					if (cardTypes.Length > 2) cardTypes = cardTypes.Substring(0, cardTypes.Length - 2);
					return cardTypes;
				}))
				.ForMember(m => m.TopLevelLaneIds, opt => opt.ResolveUsing(b =>
				{
					var laneIds = b.TopLevelLaneIds.Aggregate("", (current, laneId) => current + (laneId.GetValueOrDefault() + ", "));
					if (laneIds.Length > 2) laneIds = laneIds.Substring(0, laneIds.Length - 2);
					return laneIds;
				}));

			Mapper.CreateMap<Lane, LaneView>()
				.ForMember(m => m.Cards, opt => opt.ResolveUsing(lane => lane.Cards.Count))
				.ForMember(m => m.ChildLaneIds, opt => opt.ResolveUsing(lane =>
				{
					var laneIds = lane.ChildLaneIds.Aggregate("", (current, laneId) => current + (laneId + ", "));
					if (laneIds.Length > 2) laneIds = laneIds.Substring(0, laneIds.Length - 2);
					return laneIds;
				}))
				.ForMember(m => m.SiblingLaneIds, opt => opt.ResolveUsing(lane =>
				{
					var laneIds = lane.SiblingLaneIds.Aggregate("", (current, laneId) => current + (laneId + ", "));
					if (laneIds.Length > 2) laneIds = laneIds.Substring(0, laneIds.Length - 2);
					return laneIds;
				}));

			Mapper.CreateMap<CardView, MappedCardView>()
				.ForMember(m => m.Type, opt => opt.ResolveUsing(card => card.TypeName))
				.ForMember(m => m.AssignedUsers, opt => opt.ResolveUsing(lane =>
				{
					var users = lane.AssignedUsers.Aggregate("", (current, user) => current + (user.AssignedUserName + ", "));
					if (users.Length > 2) users = users.Substring(0, users.Length - 2);
					return users;
				}))
				.ForMember(m => m.ClassOfService, opt => opt.ResolveUsing(card => card.ClassOfServiceTitle));
		}
	}
}
