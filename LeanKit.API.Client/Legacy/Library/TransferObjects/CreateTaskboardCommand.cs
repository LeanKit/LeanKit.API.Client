namespace LeanKit.API.Legacy.Library.TransferObjects
{
	public class CreateTaskBoardCommand
	{
		public long BoardId { get; set; }
		public long ContainingCardId { get; set; }
		public long TemplateId { get; set; }
		public long CardContextId { get; set; }
	}
}