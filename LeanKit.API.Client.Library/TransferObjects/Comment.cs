//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace LeanKit.API.Client.Library.TransferObjects
{
	public class Comment
	{
		public long Id { get; set; }

		[Required(ErrorMessage = "Text may not be empty")]
		[StringLength(4000, ErrorMessage = "Text may have 2000 symbols only")]
		public string Text { get; set; }

		public string PostDate { get; set; }
		public string PostedByGravatarLink { get; set; }
		public long PostedById { get; set; }
		public string PostedByFullName { get; set; }
		public bool Editable { get; set; }
	}
}