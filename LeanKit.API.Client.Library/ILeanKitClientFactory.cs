//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using LeanKit.API.Client.Library.TransferObjects;

namespace LeanKit.API.Client.Library
{
	/// <summary>
	///     The ILeanKitClientFactory is responsible for creating an ILeanKitApi, pre-populated with
	///     data from the LeanKitAccountAuth object. The factory exists to ease the "pain" of instantiating
	///     the the default implementation of ILeanKitApi.
	/// </summary>
	public interface ILeanKitClientFactory
	{
		ILeanKitApi Create(ILeanKitAccountAuth accountAuth);

		[Obsolete("This is deprecated. Please use ILeanKitAccountAuth instead.")]
		ILeanKitApi Create(LeanKitAccountAuth accountAuth);
	}
}