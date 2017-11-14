﻿namespace LeanKit.API.Legacy.Library.TransferObjects
{
	public class DrillThroughStatisticsCard
	{
		public long Id { get; set; }
		public bool IsBlocked { get; set; }
		public int Size { get; set; }
		public string PlannedStartDate { get; set; }
		public string PlannedFinishDate { get; set; }
		public string ActualStartDate { get; set; }
		public string ActualFinishDate { get; set; }
	}
}