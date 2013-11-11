//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

namespace LeanKit.API.Client.Library.TransferObjects
{
	public class TaskboardDeleteResult
	{
		public long? NewDefaultTaskboardId { get; set; }
		public string NewCardContext { get; set; }
		public long BoardVersion { get; set; }
	}
}