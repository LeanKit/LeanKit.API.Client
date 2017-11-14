using System;

namespace LeanKit.API.Legacy.Library.TransferObjects
{
	public class BoardListing
	{
		public long Id { get; set; }
		public string Title { get; set; }
		public string IsArchived { get; set; }
		public DateTime CreationDate { get; set; }
	}
}