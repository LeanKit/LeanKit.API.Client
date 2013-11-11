//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

namespace LeanKit.API.Client.Library.TransferObjects
{
	public class BoardRole
	{
		public virtual long BoardId { get; set; }
		public virtual long RoleId { get; set; }
		public virtual long UserId { get; set; }
		public virtual string RoleName { get; set; }
		public virtual int WIP { get; set; }
	}
}