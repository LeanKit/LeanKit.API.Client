namespace LeanKit.API.Legacy.Library.TransferObjects
{
	public class AssignedUserInfo
	{
		public virtual string GravatarLink { get; set; }
		public virtual string SmallGravatarLink { get; set; }

		// These values are on board updates
		private long? _id;
		public virtual long? Id
		{
			get { return _id; }
			set 
			{ 
				_id = value;
				if (!AssignedUserId.HasValue)
				{
					AssignedUserId = value;
				}
			}
		}

		private string _emailAddress;
		public virtual string EmailAddress
		{
			get { return _emailAddress; }
			set 
			{ 
				_emailAddress = value;
				if (string.IsNullOrEmpty(AssignedUserName))
				{
					AssignedUserName = value;
				}
			}
		}
		public virtual string FullName { get; set; }

		// Are thise used?
		public virtual string AssignedUserName { get; set; }
		public virtual long? AssignedUserId { get; set; } 
	}
}