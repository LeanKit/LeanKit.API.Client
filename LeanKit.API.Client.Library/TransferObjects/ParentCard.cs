//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

namespace LeanKit.API.Client.Library.TransferObjects
{
	public class ParentCard : Card
	{
		public bool HasPermission { get; set; }
		public bool IsArchived { get; set; }
	}
}