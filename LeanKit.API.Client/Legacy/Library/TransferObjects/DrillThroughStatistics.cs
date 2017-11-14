namespace LeanKit.API.Legacy.Library.TransferObjects
{
	public class DrillThroughStatistics 
	{
		public DrillThroughStatisticsCard ParentCard { get; set; }
		public bool IsDeleted { get; set; }
		public int TotalNumberOfCards { get; set; }
		public int NumberOfCardsNotStarted { get; set; }
		public int NumberOfCardsStarted { get; set; }
		public int NumberOfCardsCompleted { get; set; }
		public int BlockedCards { get; set; }
		public int NumberOfCardsPastDue { get; set; }
		public string ProjectedStartDate { get; set; }
		public string ProjectedFinishDate { get; set; }
		public string ActualStartDate { get; set; }
		public string ActualFinishDate { get; set; }
		public bool IsAnyCardProjectedFinishDatePastParentCardProjectedFinishedDate { get; set; }
		public int TotalSizeOfCards { get; set; }
		public int TotalSizeOfCardsNotStarted { get; set; }
		public int TotalSizeOfCardsStarted { get; set; }
		public int TotalSizeOfCardsCompleted { get; set; }
		public double TotalProgressPercentage { get; set; }
	}
}