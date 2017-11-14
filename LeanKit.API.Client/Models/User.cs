using System;
using System.Collections.Generic;

namespace LeanKit.Models
{
    public class UserListRequest
    {
        public int? Offset { get; set; }
        public int? Limit { get; set; }
    }

	public class UserListResponse
	{
		public UserListResponse()
		{
			Users = new List<User>();
		}
		public PageMeta PageMeta { get; set; }
		public List<User> Users { get; set; }

		public class User
		{
			public long Id { get; set; }
			public long OrganizationId { get; set; }
			public string Username { get; set; }
			public string FirstName { get; set; }
			public string LastName { get; set; }
			public string FullName { get; set; }
			public string EmailAddress { get; set; }
			public DateTime? LastAccess { get; set; }
			public string DateFormat { get; set; }
			public bool Administrator { get; set; }
			public bool Enabled { get; set; }
			public bool Deleted { get; set; }
			public string Avatar { get; set; }
		}
	}

    public class UserRecentBoardsResponse
    {
        public List<Board> Boards { get; set; }

        public class Board 
        {
            public long Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public bool IsWelcome { get; set; }
            public bool IsArchived { get; set; }
            public string BoardRole { get; set; }
        }
    }

    public class UserResponse
    {
        public UserResponse()
        {
            BoardRoles = new List<BoardRole>();
        }
        public UserSettings Settings { get; set; }
        public List<BoardRole> BoardRoles { get; set; }
        public long Id { get; set; }
        public long OrganizationId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime LastAccess { get; set; }
        public string DateFormat { get; set; }
        public bool Administrator { get; set; }
        public bool Enabled { get; set; }
        public bool Deleted { get; set; }
        public string Avatar { get; set; }

        public class BoardRole
        {
            public long BoardId { get; set; }
            public int Wip { get; set; }
            public Role Role { get; set; }
        }

        public class Role
        {
            public string Key { get; set; }
            public int Value { get; set; }
            public string Label { get; set; }
        }

        public class UserSettings
        {
            public long[] RecentBoards { get; set; }
        }
	}
}
