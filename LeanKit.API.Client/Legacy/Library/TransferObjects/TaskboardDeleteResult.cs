namespace LeanKit.API.Legacy.Library.TransferObjects
{
	public class TaskboardDeleteResult
	{
		public long? NewDefaultTaskboardId { get; set; }
		public string NewCardContext { get; set; }
		public long BoardVersion { get; set; }
	}
}