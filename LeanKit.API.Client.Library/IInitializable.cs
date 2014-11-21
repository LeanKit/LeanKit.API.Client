//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using LeanKit.API.Client.Library.TransferObjects;

namespace LeanKit.API.Client.Library
{
	public interface ILeanKitClient
	{
		[Obsolete("This is deprecated. Please use ILeanKitAccountAuth instead.")]
		ILeanKitApi Initialize(LeanKitAccountAuth accountAuth);
		ILeanKitApi Initialize(ILeanKitAccountAuth accountAuth);
	}
}