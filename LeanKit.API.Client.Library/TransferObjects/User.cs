//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;

namespace LeanKit.API.Client.Library.TransferObjects
{
	public class User
	{
		public virtual long Id { get; set; }
		public virtual string FullName { get; set; }
		public virtual string UserName { get; set; }
		public virtual int Role { get; set; }
		public virtual string RoleName { get; set; }
		public virtual int WIP { get; set; }
		public virtual bool Enabled { get; set; }
		public virtual bool IsAccountOwner { get; set; }
		public virtual bool IsDeleted { get; set; }
		public virtual string GravatarFeed { get; set; }
		public virtual string EmailAddress { get; set; }
		public virtual string DateFormat { get; set; }
		public virtual string GravatarLink { get; set; }
		public virtual IDictionary<string, string> Settings { get; set; }

		public AssignedUserInfo ToAssignedUserInfo()
		{
			var info = new AssignedUserInfo();
			info.AssignedUserId = Id;
			info.AssignedUserName = UserName;
			info.GravatarLink = GravatarLink;
			info.SmallGravatarLink = GravatarLink;

			return info;
		}
	}
}