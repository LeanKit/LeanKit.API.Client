using System;
namespace LeanKit.Models
{
	public class AccountResponse
	{
		public long Id { get; set; }
		public int UserLimit { get; set; }
		public string Country { get; set; }
		public string Region { get; set; }
		public bool EnableCumulativeFlowDiagram { get; set; }
		public bool EnableCycleTimeDiagram { get; set; }
		public bool EnableCardDistributionDiagram { get; set; }
		public bool EnableEfficiencyDiagram { get; set; }
		public bool EnableProcessControlDiagram { get; set; }
		public bool EnableAdvancedRoleSecurity { get; set; }
		public bool EnableDrillThroughBoards { get; set; }
		public bool EnableSameBoardConnections { get; set; }
		public bool EnableMultipleDrillThroughBoards { get; set; }
		public int DefaultRoleId { get; set; }
		public int NumberOfDaysToRetrieveAnalyticsEventsFor { get; set; }
		public bool EnableSharedBoards { get; set; }
		public bool EnableConnectedCardsGallery { get; set; }
		public int DefaultNewBoardRole { get; set; }
		public long ZendeskDropboxId { get; set; }
		public string AccountType { get; set; }
		public string AccountStatus { get; set; }
		public string ExpiresOn { get; set; }
		public int ReportingApiTokenExpirationInMinutes { get; set; }
		public int ReportingApiResponseCacheDurationInMinutes { get; set; }
		public bool EnableReportingApiCardExport { get; set; }
		public bool EnableReportingApiCardLaneHistory { get; set; }
		public bool EnableReportingApiCurrentUserAssignments { get; set; }
		public bool EnableReportingApiHistoricalUserAssignments { get; set; }
		public bool EnableReportingApiLanes { get; set; }
		public bool EnableReportingApiTags { get; set; }
		public bool EnableReportingApiTasks { get; set; }
		public bool EnableReportingApiTaskLanes { get; set; }
		public bool EnableReportingApiCustomFields { get; set; }
		public bool EnableExportBoardHistory { get; set; }
	}
}
