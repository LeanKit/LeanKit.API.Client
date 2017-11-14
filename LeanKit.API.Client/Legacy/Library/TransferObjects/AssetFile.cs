namespace LeanKit.API.Legacy.Library.TransferObjects
{
	public class AssetFile
	{
		public byte[] FileBytes { get; set; }
		public string ContentType { get; set; }
		public long ContentLength { get; set; }
	}
}