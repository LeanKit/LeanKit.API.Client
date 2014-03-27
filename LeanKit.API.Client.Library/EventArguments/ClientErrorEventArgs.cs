//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;

namespace LeanKit.API.Client.Library.EventArguments
{
	public class ClientErrorEventArgs : EventArgs
	{
		public Exception Exception { get; set; }
		public string Message { get; set; }
	}
}
