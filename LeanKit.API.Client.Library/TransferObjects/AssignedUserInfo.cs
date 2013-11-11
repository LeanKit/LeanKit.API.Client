//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

namespace LeanKit.API.Client.Library.TransferObjects
{
	public class AssignedUserInfo
	{
		public virtual string GravatarLink { get; set; }
		public virtual string SmallGravatarLink { get; set; }
		public virtual string AssignedUserName { get; set; }
		public virtual long AssignedUserId { get; set; }
	}
}