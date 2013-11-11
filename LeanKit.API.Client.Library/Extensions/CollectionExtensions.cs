//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using LeanKit.API.Client.Library.Exceptions;
using LeanKit.API.Client.Library.TransferObjects;

namespace LeanKit.API.Client.Library.Extensions
{
	public static class CollectionExtensions
	{
		public static Card FindContainedCard(this IEnumerable<Lane> lanes, long laneId, long cardId)
		{
			var list = lanes as IList<Lane> ?? lanes.ToList();
			var candidateLane = list.FindLane(laneId);

			//Get the card from the lane
			var candidateCard = candidateLane.Cards.FirstOrDefault(card => card.Id == cardId);

			if (candidateCard != null) return candidateCard.ToCard();

			//the card may have been moved so look in all the affected lanes
			candidateCard = list.SelectMany(x => x.Cards).FirstOrDefault(card => card.Id == cardId);
			if (candidateCard == null)
			{
				throw new ItemNotFoundException(string.Format("Unable to find the Card [{0}] in the associated Lane [{1}].", cardId, laneId));
			}

			return candidateCard.ToCard();
		}

		public static Lane FindLane(this IEnumerable<Lane> lanes, long laneId)
		{
			var candidateLane = lanes.FirstOrDefault(lane => lane.Id == laneId);

			if (candidateLane == null)
			{
				throw new ItemNotFoundException(string.Format("Unable to find Lane [{0}].", laneId));
			}

			return candidateLane;
		}

		public static bool ContainsLane(this IEnumerable<Lane> lanes, long laneId)
		{
			return lanes.Any(lane => lane.Id == laneId);
		}

		public static bool ContainsCard(this IEnumerable<Lane> lanes, long cardId)
		{
			return lanes.SelectMany(lane => lane.Cards).Any(card => card.Id == cardId);
		}

		public static User FindUser(this IEnumerable<User> boardUsers, long userId)
		{
			var candidateUser = boardUsers.FirstOrDefault(user => user.Id == userId);

			if (candidateUser == null)
			{
				throw new ItemNotFoundException(string.Format("Unable to find User [{0}].", userId));
			}

			return candidateUser;
		}

		public static IEnumerable<Lane> GetFlatLanes(this IEnumerable<HierarchicalLane> lanes)
		{
			foreach (var c in lanes)
			{
				yield return c.Lane;

				if (c.ChildLanes.Count <= 0) continue;

				foreach (var lane in GetFlatLanes(c.ChildLanes))
				{
					yield return lane;
				}
			}
		}
	}
}