using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LeanKit.Models
{
    public enum Priority
    {
        [EnumMember(Value = "low")]
        Low = 0,
		[EnumMember(Value = "normal")]
		Normal = 1,
		[EnumMember(Value = "high")]
		High = 2,
		[EnumMember(Value = "critical")]
		Critical = 3
    }

    public enum LaneType
    {
		[EnumMember(Value = "ready")]
		Ready,
		[EnumMember(Value = "inProcess")]
		InProcess,
		[EnumMember(Value = "completed")]
		Completed,
		[EnumMember(Value = "untyped")]
		Untyped
    }

    public class CardListRequest
    {
        [Flags]
        public enum LaneClassTypeEnum
        {
            Active = 1,
            Backlog = 2,
            Archive = 4
        }

        public long? BoardId { get; set; }
        public DateTime? Since { get; set; }
        public bool? Deleted { get; set; }
        public long? TypeId { get; set; }
        public long? CustomIconId { get; set; }
        public int? Offset { get; set; }
        public int? Limit { get; set; }
        public List<string> ReturnFields { get; set; }
        public List<string> OmitFields { get; set; }
        public string Search { get; set; }
        public string Sort { get; set; }
        public LaneClassTypeEnum? LaneClassType { get; set; }
    }

    public class CardListResponse
    {
        public CardListResponse()
        {
            Cards = new List<CardListCardResponse>();
        }
        public PageMeta PageMeta { get; set; }
        public List<CardListCardResponse> Cards { get; set; }

		public class CardListCardResponse
		{
			public long Id { get; set; }
			public string Title { get; set; }
			public string Description { get; set; }
			public long Version { get; set; }
			public string Color { get; set; }
			public string IconPath { get; set; }
			public int Index { get; set; }
			public int Size { get; set; }
			public DateTime? PlannedStart { get; set; }
			public DateTime? PlannedFinish { get; set; }
			public DateTime? ActualStart { get; set; }
			public DateTime? ActualFinish { get; set; }
			public DateTime? CreatedOn { get; set; }
			public DateTime? ArchivedOn { get; set; }
			public DateTime? UpdatedOn { get; set; }
			public DateTime? MovedOn { get; set; }
			public MinimalBoard Board { get; set; }
			public BlockedStatusObject BlockedStatus { get; set; }
			public ConnectedCardStatsObject ConnectedCardStats { get; set; }
			public string CustomIconLabel { get; set; }
			public CustomIconObject CustomIcon { get; set; }
			public CustomIdObject CustomId { get; set; }
			public List<ExternalLink> ExternalLinks { get; set; }
			public CardLane Lane { get; set; }
			public string Priority { get; set; }
			public List<string> Tags { get; set; }
			public TaskBoardStatsObject TaskBoardStats { get; set; }
			public CardType Type { get; set; }

			public class BlockedStatusObject
			{
				public string Date { get; set; }
				public string Reason { get; set; }
				public bool IsBlocked { get; set; }

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

			public class CardType
			{
				public long Id { get; set; }
				public string Title { get; set; }
				public string CardColor { get; set; }
				public string IconColor { get; set; }
				public string IconName { get; set; }
				public string IconPath { get; set; }

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

			public class MinimalBoard
			{
				public long Id { get; set; }
				public string Title { get; set; }
				public long Version { get; set; }
				public bool IsArchived { get; set; }

			}

			public class TaskBoardStatsObject
			{
				public int TotalCount { get; set; }
				public int CompletedCount { get; set; }
				public int TotalSize { get; set; }
				public int CompletedSize { get; set; }

			}
		}
    }

	public class CardCreateRequest
	{
		[JsonConverter(typeof(Utils.BigIntConverter))]
		public long BoardId { get; set; }
		public string Title { get; set; }
		[JsonConverter(typeof(Utils.BigIntConverter))]
		public long TypeId { get; set; }
        [JsonConverter(typeof(Utils.BigIntArrayConverter))]
		public List<long> AssignedUserIds { get; set; }
		public string BlockReason { get; set; }
		[JsonConverter(typeof(Utils.BigIntConverter))]
		public long? CopiedFromCardId { get; set; }
		[JsonConverter(typeof(Utils.BigIntConverter))]
		public long? CustomIconId { get; set; }
		public string CustomId { get; set; }
		public string Description { get; set; }
		public ExternalLinkObject ExternalLink { get; set; }
		public int? Index { get; set; }
		[JsonConverter(typeof(Utils.BigIntConverter))]
		public long? LaneId { get; set; }
		[JsonConverter(typeof(Utils.BigIntConverter))]
		public long? ParentCardId { get; set; }
		public string PlannedStart { get; set; }
		public string PlannedFinish { get; set; }
		[JsonConverter(typeof(StringEnumConverter))]
		public Priority? Priority { get; set; }
		public int? Size { get; set; }
		public List<string> Tags { get; set; }
		public string WipOverrideComment { get; set; }
		public bool? EnforceWip { get; set; }
		public string CustomFields { get; set; }

		public class CustomField
		{
			public long FieldId { get; set; }
			public string Value { get; set; }

		}

		public class ExternalLinkObject
		{
			public string Label { get; set; }
			public string Url { get; set; }

		}
	}

	public class CardCreateResponse
	{
		public long Id { get; set; }
	}

	public class CardUpdateOperation
	{
		[JsonConverter(typeof(StringEnumConverter))]
		public enum OperationEnum
		{
			[EnumMember(Value = "add")]
			Add,
			[EnumMember(Value = "replace")]
			Replace,
			[EnumMember(Value = "remove")]
			Remove,
			[EnumMember(Value = "test")]
			Test
		}

		[JsonProperty("op")]
		public OperationEnum Operation { get; set; }
		public string Path { get; set; }
		public string Value { get; set; }
	}

	public class CardResponse
	{
		public long Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public long Version { get; set; }
		public string Color { get; set; }
		public string IconPath { get; set; }
		public int Index { get; set; }
		public int Size { get; set; }
		public DateTime? PlannedStart { get; set; }
		public DateTime? PlannedFinish { get; set; }
		public DateTime? ActualStart { get; set; }
		public DateTime? ActualFinish { get; set; }
		public DateTime? CreatedOn { get; set; }
		public DateTime? ArchivedOn { get; set; }
		public DateTime? UpdatedOn { get; set; }
		public DateTime? MovedOn { get; set; }
		public MinimalBoard Board { get; set; }
		public BlockedStatusObject BlockedStatus { get; set; }
		public ConnectedCardStatsObject ConnectedCardStats { get; set; }
		public string CustomIconLabel { get; set; }
		public CustomIconObject CustomIcon { get; set; }
		public CustomIdObject CustomId { get; set; }
		public List<ExternalLink> ExternalLinks { get; set; }
		public CardLane Lane { get; set; }
		public string Priority { get; set; }
		public List<string> Tags { get; set; }
		public TaskBoardStatsObject TaskBoardStats { get; set; }
		public CardType Type { get; set; }
		public UserObject CreatedBy { get; set; }
		public UserObject UpdatedBy { get; set; }
		public UserObject MovedBy { get; set; }
		public UserObject ArchivedBy { get; set; }
		public List<ParentCard> ParentCards { get; set; }
		public List<CustomField> CustomFields { get; set; }
		public long? SubscriptionId { get; set; }
        public List<Comment> Comments { get; set; }
		public List<UserObject> AssignedUsers { get; set; }
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
        }

		public class BlockedStatusObject
		{
			public string Date { get; set; }
			public string Reason { get; set; }
			public bool IsBlocked { get; set; }

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
			public string TaskBoard { get; set; }

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

		public class MinimalBoard
		{
			public long Id { get; set; }
			public string Title { get; set; }
			public long Version { get; set; }
			public bool IsArchived { get; set; }

		}

		public class TaskBoardStatsObject
		{
			public int TotalCount { get; set; }
			public int CompletedCount { get; set; }
			public int TotalSize { get; set; }
			public int CompletedSize { get; set; }

		}

		public class ParentCard
		{
			public long CardId { get; set; }
			public long BoardId { get; set; }

		}

		public class CustomField
		{
			public long FieldId { get; set; }
			public string Type { get; set; }
			public string Label { get; set; }
			public string Value { get; set; }

		}
	}

	public class CardCommentListResponse
	{
		public CardCommentListResponse()
		{
			Comments = new List<CardCommentResponse>();
		}
		public List<CardCommentResponse> Comments { get; set; }
	}

    public class CardCommentResponse 
    {
		public long Id { get; set; }
		public DateTime CreatedOn { get; set; }
		public UserObject CreatedBy { get; set; }
		public string Text { get; set; }

		public class UserObject
		{
			public long Id { get; set; }
			public string EmailAddress { get; set; }
			public string Avatar { get; set; }
			public string FullName { get; set; }
		}
	}

    public class CardCommentCreateRequest
    {
        public string Text { get; set; }
    }

    public class CardAttachmentListResponse
    {
        public CardAttachmentListResponse()
        {
            Attachments = new List<AttachmentResponse>();
        }
        public List<AttachmentResponse> Attachments { get; set; }
    }

	public class AttachmentResponse
	{
		public long Id { get; set; }
		public int AttachmentSize { get; set; }
		public UserObject CreatedBy { get; set; }
		public UserObject ChangedBy { get; set; }
		public DateTime CreatedOn { get; set; }
		public string Description { get; set; }
		public DateTime UpdatedOn { get; set; }
		public string Name { get; set; }
		public Guid StorageId { get; set; }

		public class UserObject
		{
			public long Id { get; set; }
			public string EmailAddress { get; set; }
			public string Avatar { get; set; }
			public string FullName { get; set; }

		}
	}

    public class AttachmentFileResponse
    {
        public byte[] FileBytes { get; set; }
        public string ContentType { get; set; }
        public long ContentLength { get; set; }
    }
}
