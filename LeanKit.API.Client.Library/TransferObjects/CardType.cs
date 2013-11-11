//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace LeanKit.API.Client.Library.TransferObjects
{
	public class CardType
	{
		public long Id { get; set; }

		[Required]
		public string Name { get; set; }

		public string ColorHex { get; set; }
		public bool IsDefault { get; set; }
		public string IconPath { get; set; }
	}
}