namespace LeanKit.API.Legacy.Library.TransferObjects
{
	public class LegacyResponse
	{
		public int ReplyCode { get; set; }
		public string ReplyText { get; set; }
		public object[] ReplyData { get; set; }
	}
}