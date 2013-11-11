//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;

namespace LeanKit.API.Client.Library.TransferObjects
{
	public class BoardIdentifiers
	{
		public BoardIdentifiers()
		{
			CardTypes = new List<Identifier>();
			BoardUsers = new List<Identifier>();
			Lanes = new List<LaneIdentifier>();
			ClassesOfService = new List<Identifier>();
			TaskboardCategories = new List<TaskboardCategoryIdentifier>();
			PopulatePriorities();
		}

		public long BoardId { get; set; }
		public List<Identifier> CardTypes { get; set; }
		public List<Identifier> BoardUsers { get; set; }
		public List<LaneIdentifier> Lanes { get; set; }
		public List<Identifier> ClassesOfService { get; set; }
		public List<Identifier> Priorities { get; set; }
		public List<TaskboardCategoryIdentifier> TaskboardCategories { get; set; }

		private void PopulatePriorities()
		{
			Priorities = new List<Identifier>
			{
				new Identifier(3, "Critical"),
				new Identifier(2, "High"),
				new Identifier(1, "Normal"),
				new Identifier(0, "Low")
			};
		}
	}

	public class Identifier
	{
		public Identifier()
		{
		}

		public Identifier(long id, string name)
		{
			Id = id;
			Name = name;
		}

		public long Id { get; set; }
		public string Name { get; set; }
	}

	public class TaskboardCategoryIdentifier : Identifier
	{
		public TaskboardCategoryIdentifier(long id, string name, bool isDefault)
			: base(id, name)
		{
			IsDefault = isDefault;
		}

		public bool IsDefault { get; set; }
	}

	public class LaneIdentifier : Identifier
	{
		public LaneIdentifier(long id, string name, long? activityId, LaneClassType laneClassType, LaneType laneType,
			long? parentLaneId, long? topLevelParentLaneId, int cardLimit, int index)
			: base(id, name)
		{
			ActivityId = activityId;
			LaneClassType = laneClassType;
			LaneType = laneType;
			ParentLaneId = parentLaneId;
			TopLevelParentLaneId = topLevelParentLaneId;
			CardLimit = cardLimit;
			Index = index;
		}

		public long? ActivityId { get; set; }
		public LaneClassType LaneClassType { get; set; }
		public LaneType LaneType { get; set; }
		public LaneClassType ClassType { get; set; }
		public LaneType Type { get; set; }
		public long? ParentLaneId { get; set; }
		public long? TopLevelParentLaneId { get; set; }
		public int CardLimit { get; set; }
		public int Index { get; set; }
	}
}