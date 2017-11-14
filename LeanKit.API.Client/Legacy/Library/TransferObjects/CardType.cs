namespace LeanKit.API.Legacy.Library.TransferObjects
{
	public class CardType
	{
		public long Id { get; set; }
		public string Name { get; set; }

		public string ColorHex { get; set; }
		public bool IsDefault { get; set; }
		public string IconPath { get; set; }
	}
}