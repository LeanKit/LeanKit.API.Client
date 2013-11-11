//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using ServiceStack.Text;

namespace LeanKitCmdQuery
{
	public static class Extensions
	{
		public static string ToCsv<T>(this IEnumerable<T> records)
		{
			return CsvSerializer.SerializeToCsv(records);
		}
	}
}