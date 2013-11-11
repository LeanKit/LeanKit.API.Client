//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;

namespace LeanKit.API.Client.Library.EventArguments
{
	public class BoardStatusCheckedEventArgs : EventArgs
	{
		public bool HasChanges { get; set; }
	}
}