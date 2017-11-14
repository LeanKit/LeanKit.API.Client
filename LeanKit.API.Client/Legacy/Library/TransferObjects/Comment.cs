namespace LeanKit.API.Legacy.Library.TransferObjects
{
	public class Comment
	{
		public long Id { get; set; }
		public string Text { get; set; }
		public string PostDate { get; set; }
		public string PostedByGravatarLink { get; set; }
		public long PostedById { get; set; }
		public string PostedByFullName { get; set; }
		public bool Editable { get; set; }
	}
}