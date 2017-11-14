namespace LeanKit.API.Legacy.Library.TransferObjects
{
	public class Asset
	{
		public long Id { get; set; }
		public string FileName { get; set; }
		public string Description { get; set; }
		public string CreatedOn { get; set; }
		public string LastModified { get; set; }
		public string StorageId { get; set; }
		public int AttachmentSize { get; set; }
		public long CardId { get; set; }
		public long CreatedById { get; set; }
		public string CreatedByFullName { get; set; }
		public long LastModifiedById { get; set; }
		public string LastModifiedByFullName { get; set; }
		public string GravatarLink { get; set; }
	}
}
