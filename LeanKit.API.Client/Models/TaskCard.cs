using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LeanKit.Models
{
	public class TaskCardListRequest
	{
		public int? Offset { get; set; }
		public int? Limit { get; set; }
	}

	public class TaskCardListResponse
    {
		public PageMeta PageMeta { get; set; }
		public List<TaskCardListCard> Cards { get; set; }
	}

	public class TaskCardListCard
	{
		public long Id { get; set; }
		public string Title { get; set; }
        public int Index { get; set; }
		public long LaneId { get; set; }
		public string Color { get; set; }
		public List<string> Tags { get; set; }
		public int Size { get; set; }
		public string Priority { get; set; }
		public DateTime? PlannedStart { get; set; }
		public DateTime? PlannedFinish { get; set; }
        public bool IsDone { get; set; }
		public DateTime UpdatedOn { get; set; }
		public DateTime MovedOn { get; set; }
		public string CustomIconLabel { get; set; }
		public BlockedStatusObject BlockedStatus { get; set; }
		public CustomIconObject CustomIcon { get; set; }
        public CustomHeaderObject CustomHeader { get; set; }
        public bool CanView { get; set; }
        public long ContainingCardId { get; set; }
		public CardTypeObject CardType { get; set; }
        public List<UserObject> AssignedUsers { get; set; }
        public long? SubscriptionId { get; set; }
        public ParentCardObject ParentCard { get; set; }
        public ConnectedCardStatsObject ConnectedCardStats { get; set; }

		public class UserObject
		{
			public long Id { get; set; }
			public string Avatar { get; set; }
			public string FullName { get; set; }
		}

        public class CustomHeaderObject
        {
            public string Value { get; set; }
            public string Header { get; set; }
            public string Url { get; set; }
        }

		public class BlockedStatusObject
		{
			public DateTime? Date { get; set; }
			public string Reason { get; set; }
			public bool IsBlocked { get; set; }
		}

        public class ParentCardObject
        {
            public long CardId { get; set; }
            public long BoardId { get; set; }
        }

		public class CardTypeObject
		{
			public long Id { get; set; }
			public string Name { get; set; }
		}

		public class CardLane
		{
			public long Id { get; set; }
			public string Title { get; set; }
			public string Description { get; set; }
			public int Index { get; set; }
			public int CardLimit { get; set; }
			public string LaneClassType { get; set; }
			public string LaneType { get; set; }
			public string Orientation { get; set; }
		}

		public class BoardMinimal
		{
			public long Id { get; set; }
			public string Title { get; set; }
			public long Version { get; set; }
			public bool IsArchived { get; set; }
		}

		public class CustomIconObject
		{
			public long Id { get; set; }
			public string Title { get; set; }
			public string CardColor { get; set; }
			public string IconColor { get; set; }
			public string IconName { get; set; }
			public string IconPath { get; set; }
			public string Policy { get; set; }
		}

		public class CustomIdObject
		{
			public string Value { get; set; }
			public string Prefix { get; set; }
			public string Url { get; set; }
		}

		public class ExternalLink
		{
			public string Label { get; set; }
			public string Url { get; set; }
		}

		public class TaskBoardObject
		{
			public long Id { get; set; }
			public long Version { get; set; }
		}

		public class ConnectedCardStatsObject
		{
			public int StartedCount { get; set; }
			public int StartedSize { get; set; }
			public int NotStartedCount { get; set; }
			public int NotStartedSize { get; set; }
			public int CompletedCount { get; set; }
			public int CompletedSize { get; set; }
			public int BlockedCount { get; set; }
			public int TotalCount { get; set; }
			public int TotalSize { get; set; }
			public DateTime? PlannedStart { get; set; }
			public DateTime? PlannedFinish { get; set; }
			public DateTime? ActualStart { get; set; }
			public DateTime? ActualFinish { get; set; }
			public int PastDueCount { get; set; }
			public int ProjectedLateCount { get; set; }
		}
	}

	public class TaskCardResponse
	{
		public long Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public int Index { get; set; }
		public List<string> Tags { get; set; }
		public int Size { get; set; }
		public long Version { get; set; }
		public string Priority { get; set; }
		public DateTime CreatedOn { get; set; }
		public DateTime? ArchivedOn { get; set; }
		public DateTime? PlannedStart { get; set; }
		public DateTime? PlannedFinish { get; set; }
		public DateTime? ActualStart { get; set; }
		public DateTime? ActualFinish { get; set; }
		public DateTime UpdatedOn { get; set; }
		public DateTime MovedOn { get; set; }
		public string Color { get; set; }
		public string IconPath { get; set; }
		public BlockedStatusObject BlockedStatus { get; set; }
		public BoardMinimal Board { get; set; }
		public TaskBoardObject TaskBoard { get; set; }
		public CustomIconObject CustomIcon { get; set; }
		public CustomIdObject CustomId { get; set; }
		public List<ExternalLink> ExternalLinks { get; set; }
		public CardLane Lane { get; set; }
		public CardType Type { get; set; }
        public List<UserObject> AssignedUsers { get; set; }
        public UserObject CreatedBy { get; set; }
        public UserObject UpdatedBy { get; set; }
		public UserObject MovedBy { get; set; }
		public UserObject ArchivedBy { get; set; }
		public List<Comment> Comments { get; set; }
        public List<Attachment> Attachments { get; set; }

		public class Comment
		{
			public long Id { get; set; }
			public string Text { get; set; }
			public UserObject CreatedBy { get; set; }
			public DateTime CreatedOn { get; set; }
		}

		public class Attachment
		{
			public long Id { get; set; }
            public int AttachmentSize { get; set; }
			public string Name { get; set; }
            public string Description { get; set; }
            public string StorageId { get; set; }
			public UserObject CreatedBy { get; set; }
            public UserObject ChangedBy { get; set; }
			public DateTime CreatedOn { get; set; }
			public DateTime UpdatedOn { get; set; }
		}

		public class UserObject
		{
			public long Id { get; set; }
			public string EmailAddress { get; set; }
			public string Avatar { get; set; }
			public string FullName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
		}


		public class BlockedStatusObject
		{
			public string Date { get; set; }
			public string Reason { get; set; }
			public bool IsBlocked { get; set; }
		}

		public class CardType
		{
			public long Id { get; set; }
			public string Title { get; set; }
			public string CardColor { get; set; }
			public string IconColor { get; set; }
			public string IconName { get; set; }
			public string IconPath { get; set; }
		}

		public class CardLane
		{
			public long Id { get; set; }
			public string Title { get; set; }
			public string Description { get; set; }
			public int Index { get; set; }
			public int CardLimit { get; set; }
			public string LaneClassType { get; set; }
			public string LaneType { get; set; }
			public string Orientation { get; set; }
		}

		public class BoardMinimal
		{
			public long Id { get; set; }
			public string Title { get; set; }
			public long Version { get; set; }
			public bool IsArchived { get; set; }
		}

		public class CustomIconObject
		{
			public long Id { get; set; }
			public string Title { get; set; }
			public string CardColor { get; set; }
			public string IconColor { get; set; }
			public string IconName { get; set; }
			public string IconPath { get; set; }
			public string Policy { get; set; }
		}

		public class CustomIdObject
		{
			public string Value { get; set; }
			public string Prefix { get; set; }
			public string Url { get; set; }
		}

		public class ExternalLink
		{
			public string Label { get; set; }
			public string Url { get; set; }
		}

        public class TaskBoardObject
        {
            public long Id { get; set; }
            public long Version { get; set; }
        }
	}

    public class TaskCardCreateRequest
    {
        [JsonConverter(typeof(Utils.BigIntConverter))]
        public long CardId { get; set; }
		[JsonConverter(typeof(Utils.BigIntConverter))]
		public long TypeId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string LaneTitle { get; set; }
		[JsonConverter(typeof(StringEnumConverter))]
		public LaneType? LaneType { get; set; }
		public int? Index { get; set; }
        public int? Size { get; set; }
		[JsonConverter(typeof(StringEnumConverter))]
		public Priority? Priority { get; set; }
        [JsonConverter(typeof(Utils.DateOnlyConverter))]
        public DateTime? PlannedStart { get; set; }
		[JsonConverter(typeof(Utils.DateOnlyConverter))]
		public DateTime? PlannedFinish { get; set; }
        public string BlockReason { get; set; }
		[JsonConverter(typeof(Utils.BigIntArrayConverter))]
		public List<long> AssignedUserIds { get; set; }
		public List<string> Tags { get; set; }
        public string CustomId { get; set; }
		[JsonConverter(typeof(Utils.BigIntConverter))]
		public long? CustomIconId { get; set; }
        public ExternalLinkObject ExternalLink { get; set; }
        public string WipOverrideComment { get; set; }

		public class ExternalLinkObject
		{
			public string Label { get; set; }
			public string Url { get; set; }
		}
	}
}
