//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;

namespace LeanKit.API.Client.Library.TransferObjects
{
	public class Taskboard
	{
		public virtual long Id { get; set; }
		public virtual string Title { get; set; }

		public virtual long Version { get; set; }
		public virtual bool Active { get; set; }
		public virtual IList<Lane> Lanes { get; set; }
		public virtual IList<long?> TopLevelLaneIds { get; set; }
		public virtual CardContext CardContext { get; set; }
		public int ProgressPercentage { get; set; }
		public long BoardVersion { get; set; }
	}
}