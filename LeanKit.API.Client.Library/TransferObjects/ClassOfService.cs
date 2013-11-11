//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace LeanKit.API.Client.Library.TransferObjects
{
	public class ClassOfService
	{
		public virtual long Id { get; set; }
		public virtual long BoardId { get; set; }

		[Required]
		public virtual string Title { get; set; }

		public virtual string Policy { get; set; }
		public virtual string IconPath { get; set; }
		public string ColorHex { get; set; }
		public bool UseColor { get; set; }
	}
}