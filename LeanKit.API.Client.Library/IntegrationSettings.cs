//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

namespace LeanKit.API.Client.Library
{
	public class IntegrationSettings
	{
		public IntegrationSettings()
		{
			CheckForUpdatesIntervalSeconds = 5;
			TotalBoardRefeshIntervalMinutes = 1440;
			DateFormat = "MM/dd/yyyy";
		}

		//TODO:  use this to pass in dynamic properties 
		public int CheckForUpdatesIntervalSeconds { get; set; }
		public int TotalBoardRefeshIntervalMinutes { get; set; }
		public string DateFormat { get; set; }
	}
}