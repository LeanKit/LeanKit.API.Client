//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace LeanKit.API.Client.Library.TransferObjects
{
	public class BoardUpdates
	{
		public bool HasUpdates { get; set; }
		public List<Lane> AffectedLanes { get; set; }
		public List<BoardHistoryEvent> Events { get; set; }
		public long CurrentBoardVersion { get; set; }
		public Board NewPayload { get; set; }

		public bool RequiresRefesh()
		{
			return Events.Any(evnt => evnt.RequiresBoardRefresh);
		}
	}
}