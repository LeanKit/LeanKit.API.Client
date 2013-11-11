//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;

namespace LeanKit.API.Client.Library.EventArguments
{
	public class BoardInfoRefreshedEventArgs : EventArgs
	{
		public bool FromBoardChange { get; set; }
	}
}