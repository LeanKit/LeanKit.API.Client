namespace LeanKit.API.Legacy.Library.TransferObjects
{
	public class ClassOfService
	{
		public virtual long Id { get; set; }
		public virtual long BoardId { get; set; }
		public virtual string Title { get; set; }
		public virtual string Policy { get; set; }
		public virtual string IconPath { get; set; }
		public string ColorHex { get; set; }
		public bool UseColor { get; set; }
	}
}