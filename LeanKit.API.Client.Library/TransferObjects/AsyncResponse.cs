//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

namespace LeanKit.API.Client.Library.TransferObjects
{
	public class AsyncResponse
	{
		public int ReplyCode { get; set; }
		public string ReplyText { get; set; }
		public string ReplyData { get; set; }
	}

	public class AsyncResponse2
	{
		public int ReplyCode { get; set; }
		public string ReplyText { get; set; }
		public object[] ReplyData { get; set; }
	}
}