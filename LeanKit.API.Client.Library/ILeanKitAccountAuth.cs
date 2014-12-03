//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

namespace LeanKit.API.Client.Library
{
	public interface ILeanKitAccountAuth
	{
		string GetAccountUrl();
		string Hostname { get; set; }
	}
}
