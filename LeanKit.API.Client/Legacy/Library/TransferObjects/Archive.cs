using System.Collections.Generic;

namespace LeanKit.API.Legacy.Library.TransferObjects
{
	public class Archive
	{
		public virtual long? Id { get; set; }
		public virtual int Index { get; set; }
		public virtual bool Active { get; set; }
		public virtual string Title { get; set; }
		public virtual int CardLimit { get; set; }
		public virtual LaneClassType ClassType { get; set; }
		public virtual LaneType Type { get; set; }
		public virtual short Width { get; set; }
		public virtual long ParentLaneId { get; set; }
		public virtual IEnumerable<Card> Cards { get; set; }
	}
}