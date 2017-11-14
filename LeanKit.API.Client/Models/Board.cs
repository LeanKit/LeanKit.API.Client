using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LeanKit.Models
{
    public class BoardListRequest
    {
		public int? Offset { get; set; }
		public int? Limit { get; set; }
		public bool? InvertSort { get; set; }
		public string Search { get; set; }
		public string MinimumAccess { get; set; }
		public bool? Archived { get; set; }
    }

    public class BoardListResponse
    {
        public BoardListResponse()
        {
            Boards = new List<Board>();
        }
        public PageMeta PageMeta { get; set; }
        public List<Board> Boards { get; set; }

        public class Board
        {
            public long Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public int BoardRoleId { get; set; }
            public bool IsWelcome { get; set; }
            public string BoardRole { get; set; }
        }
    }

    public class BoardResponse
    {
        public BoardResponse()
        {
            CardTypes = new List<CardType>();
            LaneTypes = new List<LaneType>();
            LaneClassTypes = new List<LaneClassType>();
            Tags = new List<string>();
            Priorities = new List<Priority>();
            Lanes = new List<Lane>();
            Users = new List<User>();
            CustomFields = new List<CustomField>();
            ClassesOfService = new List<ClassOfService>();
        }
        public long Id { get; set; }
        public long OrganizationId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool ClassOfServiceEnabled { get; set; }
        public string CustomIconFieldLabel { get; set; }
        public long Version { get; set; }
        public int CardColorField { get; set; }
        public bool IsCardIdEnabled { get; set; }
        public bool IsHeaderEnabled { get; set; }
        public bool IsHyperlinkEnabled { get; set; }
        public bool IsPrefixEnabled { get; set; }
        public string Prefix { get; set; }
        public string Format { get; set; }
        public bool IsPrefixIncludedInHyperlink { get; set; }
        public bool BaseWipOnCardSize { get; set; }
        public bool ExcludeCompletedAndArchiveViolations { get; set; }
        public bool IsDuplicateCardIdAllowed { get; set; }
        public bool IsAutoIncrementCardIdEnabled { get; set; }
        public long? CurrentExternalCardId { get; set; }
        public bool IsWelcome { get; set; }
        public bool IsShared { get; set; }
        public long SharedBoardRole { get; set; }
        public string CustomBoardMoniker { get; set; }
        public bool IsPermalinkEnabled { get; set; }
        public bool IsExternalUrlEnabled { get; set; }
        public bool AllowUsersToDeleteCards { get; set; }
        public long? DefaultCardTypeId { get; set; }
        public long? DefaultTaskTypeId { get; set; }
        public List<CardType> CardTypes { get; set; }
        public List<LaneType> LaneTypes { get; set; }
        public List<LaneClassType> LaneClassTypes { get; set; }
        public List<string> Tags { get; set; }
        public List<Priority> Priorities { get; set; }
        public List<Lane> Lanes { get; set; }
        public List<User> Users { get; set; }
        public UserSettingsResponse UserSettings { get; set; }
        public List<CustomField> CustomFields { get; set; }
        public List<ClassOfService> ClassesOfService { get; set; }
        public string BoardRole { get; set; }
        public string EffectiveBoardRole { get; set; }

        public class CardType
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public string ColorHex { get; set; }
            public bool IsCardType { get; set; }
            public bool IsTaskType { get; set; }

        }

        public class LaneType
        {
            public int Id { get; set; }
            public string Name { get; set; }

        }

        public class LaneClassType
        {
            public int Id { get; set; }
            public string Name { get; set; }

        }

        public class Priority
        {
            public int Id { get; set; }
            public string Name { get; set; }

        }

        public class Lane
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public bool Active { get; set; }
            public int CardLimit { get; set; }
            public string CreationDate { get; set; }
            public int Index { get; set; }
            public long? ParentLaneId { get; set; }
            public long? ActivityId { get; set; }
            public string Orientation { get; set; }
            public bool IsConnectionDoneLane { get; set; }
            public bool IsDefaultDropLane { get; set; }
            public string LaneClassType { get; set; }
            public string LaneType { get; set; }
            public int Columns { get; set; }
            public int WipLimit { get; set; }
            public int CardCount { get; set; }
            public int CardSize { get; set; }
            public int ArchiveCardCount { get; set; }

        }

        public class ClassOfService
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public string IconPath { get; set; }

        }

        public class CustomField
        {
            public long Id { get; set; }
            public int Index { get; set; }
            public string Type { get; set; }
            public string Label { get; set; }
            public string HelpText { get; set; }
            public object ChoiceConfiguration { get; set; }

        }

        public class User
        {
            public User()
            {
                BoardRoles = new List<BoardRoleResponse>();
            }
            public long Id { get; set; }
            public long OrganizationId { get; set; }
            public long BoardId { get; set; }
            public string Username { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string FullName { get; set; }
            public string EmailAddress { get; set; }
            public string GravatarLink { get; set; }
            public string Avatar { get; set; }
            public string LastAccess { get; set; }
            public string DateFormat { get; set; }
            public object Settings { get; set; }
            public List<BoardRoleResponse> BoardRoles { get; set; }

        }

        public class UserSettingsResponse
        {
            public object SavedFilters { get; set; }
            public bool AutomaticSubscriptionOn { get; set; }
            public bool AvatarOn { get; set; }
            public bool DraggableBoardOn { get; set; }
            public bool EnableGrowlerUpdates { get; set; }
            public bool FilterCollapsedLanes { get; set; }
            public bool KanbanConfigOptionsEnableHotKeys { get; set; }
            public bool KanbanConfigOptionsDisableAutoRefresh { get; set; }
            public bool KanbanConfigOptionsDisableHoverMenus { get; set; }

        }

        public class BoardRoleResponse
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
    }

	public class CustomFieldListResponse
	{
		public CustomFieldListResponse()
		{
			CustomFields = new List<CustomCardField>();
		}
		public int Limit { get; set; }
		public List<CustomCardField> CustomFields { get; set; }

		public class CustomCardField
		{
			public long Id { get; set; }
			public string Label { get; set; }
			public string HelpText { get; set; }
			public string Type { get; set; }
			public int Index { get; set; }
			public ChoiceConfigurationObject ChoiceConfiguration { get; set; }
			public long CreatedBy { get; set; }
			public string CreatedOn { get; set; }

			public class ChoiceConfigurationObject
			{
				public ChoiceConfigurationObject()
				{
					Choices = new List<string>();
				}
				public List<string> Choices { get; set; }

			}
		}
	}

	public class BoardCreateRequest
	{
        [JsonProperty("templateId")]
        [JsonConverter(typeof(Utils.BigIntConverter))]
        public long? TemplateId { get; set; }

        [JsonProperty("fromBoardId")]
		[JsonConverter(typeof(Utils.BigIntConverter))]
		public long? FromBoardId { get; set; }

		[JsonProperty("includeExistingUsers")]
		public bool? IncludeExistingUsers { get; set; }
		
        [JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }
		
        [JsonProperty("baseWipOnCardSize")]
		public bool? BaseWipOnCardSize { get; set; }

		[JsonProperty("excludeCompletedAndArchiveViolations")]
		public bool? ExcludeCompletedAndArchiveViolations { get; set; }
		
        [JsonProperty("includeCards")]
		public bool? IncludeCards { get; set; }

		[JsonProperty("isShared")]
		public bool? IsShared { get; set; }
		
        [JsonProperty("sharedBoardRole")]
		public string SharedBoardRole { get; set; }
	}

    public class BoardCreateResponse
    {
        public long Id { get; set; }
    }

	public class BoardCustomFieldUpdateOperation
	{
		[JsonConverter(typeof(StringEnumConverter))]
		public enum OperationEnum
		{
			[EnumMember(Value = "add")]
			Add,
			[EnumMember(Value = "replace")]
			Replace,
			[EnumMember(Value = "remove")]
			Remove
		}

		[JsonConverter(typeof(StringEnumConverter))]
		public enum CustomFieldTypeEnum
		{
			[EnumMember(Value = "text")]
			Text,
			[EnumMember(Value = "number")]
			Number,
			[EnumMember(Value = "date")]
			Date,
			[EnumMember(Value = "choice")]
			Choice
		}

		[JsonProperty("op")]
		public OperationEnum Operation { get; set; }
		[JsonProperty("path")]
		public string Path { get; set; }
		[JsonProperty("value")]
		public UpdateOperationValue Value { get; set; }

		public class UpdateOperationValue
		{
			[JsonProperty("id")]
			[JsonConverter(typeof(Utils.BigIntConverter))]
			public long? Id { get; set; }
			[JsonProperty("label")]
			public string Label { get; set; }
			[JsonProperty("helpText")]
			public string HelpText { get; set; }
			[JsonProperty("type")]
			public CustomFieldTypeEnum? Type { get; set; }
			[JsonProperty("index")]
			public int? Index { get; set; }
			[JsonProperty("choiceConfiguration")]
			public ChoiceConfigurationObject ChoiceConfiguration { get; set; }
		}

		public class ChoiceConfigurationObject
		{
            public ChoiceConfigurationObject()
            {
                Choices = new List<string>();
            }
			[JsonProperty("choices")]
			public List<string> Choices { get; set; }
		}
	}
}
