//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LeanKit.API.Client.Library.TransferObjects
{
	[DebuggerDisplay("{GetType().Name,nq} [{Id}] {Title}")]
	public class Lane
	{
		private int? _cardLimit;

		public virtual long? Id { get; set; }
		public virtual int Index { get; set; }
		public virtual bool Active { get; set; }
		public virtual string Title { get; set; }
		public virtual string Description { get; set; }
		public virtual LaneClassType ClassType { get; set; }
		public virtual LaneType Type { get; set; }
		public virtual long? ActivityId { get; set; }
		public virtual string ActivityName { get; set; }
		public virtual long? CardContextId { get; set; }
		public virtual short Width { get; set; }
		public virtual long ParentLaneId { get; set; }
		public virtual IList<CardView> Cards { get; set; }
		public virtual Orientation Orientation { get; set; }
		public virtual List<long> ChildLaneIds { get; set; }
		public virtual List<long> SiblingLaneIds { get; set; }
		public virtual long? TaskBoardId { get; set; }

		public virtual int? CardLimit
		{
			get { return _cardLimit.GetValueOrDefault(0); }
			set { _cardLimit = value; }
		}

		public virtual string LaneState
		{
			get
			{
				if (ChildLaneIds == null)
				{
					ChildLaneIds = new List<long>();
				}
				if (ChildLaneIds.Count > 0 && ParentLaneId == 0)
					return "parent";
				if (ChildLaneIds.Count > 0 && ParentLaneId != 0)
					return "childParent";
				if (ChildLaneIds.Count == 0 && ParentLaneId == 0)
					return "lane";
				if (ChildLaneIds.Count == 0 && ParentLaneId != 0)
					return "child";
				return "lane";
			}
		}

		public void UpdateCard(CardView cardToUpdate)
		{
			int index = Cards.IndexOf(Cards.FirstOrDefault(x => x.Id == cardToUpdate.Id));
			Cards.RemoveAt(index);
			Cards.Insert(index, cardToUpdate);
		}
	}

	public enum LaneClassType
	{
		Active,
		Backlog,
		Archive
	}


	public enum LaneType
	{
		Ready = 1,
		InProcess = 2,
		Completed = 3,
		Untyped = 99,
	}

	public enum Orientation
	{
		Vertical = 0,
		Horizontal = 1
	}
}