using System.Collections.Generic;

namespace LeanKit.API.Legacy.Library.TransferObjects
{
	public class HierarchicalLane
	{
		public virtual Lane Lane { get; set; }
		public virtual ParentLane ParentLane { get; set; }
		public virtual IList<HierarchicalLane> ChildLanes { get; set; }
	}

	public class ParentLane
	{
		public virtual Lane Lane { get; set; }
		public virtual ParentLane Parent { get; set; }
	}
}