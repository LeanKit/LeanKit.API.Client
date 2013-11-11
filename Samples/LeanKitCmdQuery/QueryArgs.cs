//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using PowerArgs;

namespace LeanKitCmdQuery
{
	[ArgExample("LeanKitCmdQuery -h [account name] -u [email] -p [password] [-b [board id]]", "Query LeanKit and output results to console.")]
	public class QueryArgs
	{
		[ArgRequired(PromptIfMissing = false, Priority = 9)]
		[ArgShortcut("h")]
		[ArgDescription("LeanKit host name (e.g. CompanyName)")]
		public string Host { get; set; }

		[ArgRequired(PromptIfMissing = false, Priority = 8)]
		[ArgShortcut("u")]
		[ArgDescription("Account email address")]
		public string User { get; set; }

		[ArgRequired(PromptIfMissing = false, Priority = 7)]
		[ArgShortcut("p")]
		[ArgDescription("Account password")]
		public string Password { get; set; }

		[ArgShortcut("b")]
		[ArgDescription("Specify a board with the given identifier (ID)")]
		public long Board { get; set; }

		[ArgDescription("List all boards available to account")]
		public bool Boards { get; set; }

		[ArgDescription("List all lanes for the given board")]
		public bool Lanes { get; set; }

		[ArgShortcut("l")]
		[ArgDescription("Specify a lane with the given identifier (ID)")]
		public long Lane { get; set; }

		[ArgShortcut("backlog")]
		[ArgDescription("Include backlog lane(s)")]
		public bool IncludeBacklog { get; set; }

		[ArgShortcut("archive")]
		[ArgDescription("Include archive lane(s)")]
		public bool IncludeArchive { get; set; }

		[ArgDescription("List all cards for the given board or lane")]
		public bool Cards { get; set; }

		[ArgDescription("Output results in comma-delimited format (CSV)")]
		public bool Csv { get; set; }

		[ArgDescription("Output results in JSON format")]
		public bool Json { get; set; }
	}
}