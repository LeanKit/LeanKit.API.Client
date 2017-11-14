using System.Collections.Generic;
using System.Linq;
using LeanKit.API.Legacy.Library.Exceptions;
using Newtonsoft.Json;

namespace LeanKit.API.Legacy.Library.TransferObjects
{
	[JsonObject(MemberSerialization = MemberSerialization.OptOut)]
	public class Board
	{
		public virtual long Id { get; set; }
		public virtual string Title { get; set; }
		public virtual string Description { get; set; }
		public virtual long Version { get; set; }
		public virtual bool Active { get; set; }
		public virtual long OrganizationId { get; set; }
		public virtual IList<Activity> OrganizationActivities { get; set; }
		public virtual IList<Lane> Lanes { get; set; }
		public virtual IEnumerable<User> BoardUsers { get; set; }
		public virtual IList<Lane> Backlog { get; set; }
		public virtual IList<Lane> Archive { get; set; }
		public virtual IList<ClassOfService> ClassesOfService { get; set; }
		public virtual IList<CardType> CardTypes { get; set; }
		public virtual bool ClassOfServiceEnabled { get; set; }
		public virtual string CardColorField { get; set; }
		public virtual bool IsCardIdEnabled { get; set; }
		public virtual bool IsHeaderEnabled { get; set; }
		public virtual bool IsPrefixEnabled { get; set; }
		public virtual bool IsPrefixIncludedInHyperlink { get; set; }
		public virtual string Prefix { get; set; }
		public virtual bool IsHyperlinkEnabled { get; set; }
		public virtual string Format { get; set; }
		public virtual bool AllowMultiUserAssignments { get; set; }
		public virtual bool BaseWipOnCardSize { get; set; }
		public virtual bool ExcludeFromOrgAnalytics { get; set; }
		public virtual bool ExcludeCompletedAndArchiveViolations { get; set; }
		public virtual IList<long?> TopLevelLaneIds { get; set; }
		public virtual long? BacklogTopLevelLaneId { get; set; }
		public virtual long? ArchiveTopLevelLaneId { get; set; }
		public IList<CardContext> CardContexts { get; set; }
		public IList<ParentCard> ParentCards { get; set; }

		public IEnumerable<Lane> AllLanes()
		{
			return new List<Lane>().Concat(Backlog).Concat(Lanes).Concat(Archive);
		}


		public CardView GetCardViewById(long cardId)
		{
			CardView cardView = Lanes.Where(x => x.Cards != null).SelectMany(l => l.Cards).FirstOrDefault(c => c.Id == cardId);

			if (cardView == null)
			{
				cardView = Backlog.Where(x => x.Cards != null).SelectMany(lane => lane.Cards).FirstOrDefault(c => c.Id == cardId);
			}
			if (cardView == null)
			{
				cardView = Archive.Where(x => x.Cards != null).SelectMany(lane => lane.Cards).FirstOrDefault(c => c.Id == cardId);
			}

			return cardView;
		}

		public Card GetCardById(long cardId)
		{
			CardView cardView = GetCardViewById(cardId);

			return cardView == null ? null : cardView.ToCard();
		}

		public Card GetCardByExternalId(string externalId)
		{
			if (IsPrefixEnabled && externalId.StartsWith(Prefix))
			{
				externalId = externalId.TrimStart(Prefix.ToCharArray());
			}

			CardView cardView = Lanes.SelectMany(lane => lane.Cards).FirstOrDefault(c => c.ExternalCardID == externalId);

			if (cardView == null)
			{
				cardView = Backlog.SelectMany(lane => lane.Cards).FirstOrDefault(c => c.ExternalCardID == externalId);
			}
			if (cardView == null)
			{
				cardView = Archive.SelectMany(lane => lane.Cards).FirstOrDefault(c => c.ExternalCardID == externalId);
			}

			return cardView == null ? null : cardView.ToCard();
		}

		public Lane GetLaneById(long laneId)
		{
			Lane lane = Lanes.FirstOrDefault(x => x.Id == laneId);

			if (lane == null)
			{
				lane = Backlog.FirstOrDefault(x => x.Id == laneId);
			}

			if (lane == null)
			{
				lane = Archive.FirstOrDefault(x => x.Id == laneId);
			}

			return lane;
		}

		public void UpdateLane(Lane laneToUpdateWith)
		{
			//Try getting the lane from the Lanes
			Lane regularLaneToReplace = Lanes.FirstOrDefault(lane => lane.Id == laneToUpdateWith.Id);
			if (regularLaneToReplace != null)
			{
				int laneIndex = Lanes.IndexOf(regularLaneToReplace);
				Lanes.RemoveAt(laneIndex);
				Lanes.Insert(laneIndex, laneToUpdateWith);
				return;
			}

			//If none, try getting from Backlog
			Lane backLogLaneToReplace = Backlog.FirstOrDefault(lane => lane.Id == laneToUpdateWith.Id);
			if (backLogLaneToReplace != null)
			{
				int laneIndex = Backlog.IndexOf(backLogLaneToReplace);
				Backlog.RemoveAt(laneIndex);
				Backlog.Insert(laneIndex, laneToUpdateWith);
				return;
			}

			//Lastly, get it from Archive
			Lane archiveLaneToReplace = Archive.FirstOrDefault(lane => lane.Id == laneToUpdateWith.Id);
			if (archiveLaneToReplace != null)
			{
				int laneIndex = Archive.IndexOf(archiveLaneToReplace);
				Archive.RemoveAt(laneIndex);
				Archive.Insert(laneIndex, laneToUpdateWith);
				return;
			}

			throw new ItemNotFoundException("Could not find the Lane to replace with the updated Lane.");
		}

		public void ApplyCardMove(long cardId, long toLaneId, int position)
		{
			//get the card
			CardView currentCard = GetCardViewById(cardId);

			//remove from the lane it was in
			Lane lane = GetLaneById(currentCard.LaneId);
			int index = lane.Cards.IndexOf(currentCard);

			foreach (CardView cardView in lane.Cards.OrderBy(card => card.Index))
			{
				if (cardView.Index > currentCard.Index)
				{
					cardView.Index--;
				}
			}

			lane.Cards.RemoveAt(index);

			//move the card into the new lane
			bool inserted = false;
			int indexToInsert = 0;
			Lane destinationLane = GetLaneById(toLaneId);
			if (destinationLane.Cards == null) destinationLane.Cards = new List<CardView>();
			foreach (CardView cardView in destinationLane.Cards.OrderBy(card => card.Index))
			{
				if (cardView.Index >= position)
				{
					if (!inserted)
					{
						indexToInsert = destinationLane.Cards.IndexOf(cardView);
						inserted = true;
					}
					else
					{
						cardView.Index++;
					}
				}
			}

			currentCard.Index = position;
			destinationLane.Cards.Insert(indexToInsert, currentCard);
			if (destinationLane.Id != null) currentCard.LaneId = (long) destinationLane.Id;
		}

		public void ApplyCardDelete(long cardId)
		{
			CardView cardToDelete = GetCardViewById(cardId);
			IList<CardView> laneCards = GetLaneById(cardToDelete.Id).Cards;

			for (int i = 0; i < laneCards.Count(); i++)
			{
				CardView currentCard = laneCards[i];
				if (currentCard.Index > cardToDelete.Index)
				{
					currentCard.Index--;
				}
			}

			laneCards.Remove(cardToDelete);
		}

		public string GetLaneTitle(long laneId)
		{
			long parentLaneId = 0;
			Lane parentLane = Lanes.FirstOrDefault(x => x.ChildLaneIds != null && x.ChildLaneIds.Contains(laneId));

			if (parentLane != null)
			{
				parentLaneId = parentLane.Id.GetValueOrDefault(0);
			}
			if (parentLaneId != 0)
			{
				return GetLaneTitle(parentLaneId) + ":" + GetLaneById(laneId).Title;
			}
			return GetLaneById(laneId).Title;
		}
	}
}