//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

namespace LeanKit.API.Client.Library.TransferObjects
{
	public class UserAssignmentDTO
	{
		public long CardId { get; set; }
		public long UserId { get; set; }
		public string OverrideComment { get; set; }
	}
}