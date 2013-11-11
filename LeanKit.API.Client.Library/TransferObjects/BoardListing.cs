//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;

namespace LeanKit.API.Client.Library.TransferObjects
{
	public class BoardListing
	{
		public long Id { get; set; }
		public string Title { get; set; }
		public string IsArchived { get; set; }
		public DateTime CreationDate { get; set; }
	}
}