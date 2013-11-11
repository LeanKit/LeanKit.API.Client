//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using LeanKit.API.Client.Library.TransferObjects;

namespace LeanKit.API.Client.Library
{
	public interface ILeanKitClient
	{
		ILeanKitApi Initialize(LeanKitAccountAuth accountAuth);
	}
}