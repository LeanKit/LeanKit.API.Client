//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using PowerArgs;

namespace LeanKitCmdQuery
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				// Parse command line parameters
				var cmdArgs = Args.Parse<QueryArgs>(args);
				
				// Initialize mappings
				var mappings = new Mappings.Mappings();
				mappings.Init();

				// Run the query
				new LeanKitQuery(cmdArgs).RunQuery();
			}
			catch (ArgException aex)
			{
				// Invalid arguments were passed, write out help
				Console.WriteLine(aex.Message);
				ArgUsage.GetStyledUsage<QueryArgs>().Write();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Unexpected error: " + ex.Message);
			}
		}
	}
}
