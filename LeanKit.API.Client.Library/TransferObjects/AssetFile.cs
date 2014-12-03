//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

namespace LeanKit.API.Client.Library.TransferObjects
{
	public class AssetFile
	{
		public byte[] FileBytes { get; set; }
		public string ContentType { get; set; }
		public long ContentLength { get; set; }
	}
}