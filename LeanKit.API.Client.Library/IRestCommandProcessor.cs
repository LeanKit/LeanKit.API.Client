//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using LeanKit.API.Client.Library.TransferObjects;

namespace LeanKit.API.Client.Library
{
	/// <summary>
	///     The IRestCommandProcessor is responsible for communicating via HTTP with the LeanKit API server on behalf of the
	///     classes that consume it.
	/// </summary>
	public interface IRestCommandProcessor
	{
		T Get<T>(LeanKitAccountAuth accountAuth, string resource) where T : new();
		T Get<T>(LeanKitAccountAuth accountAuth, string resource, object body) where T : new();
		T Post<T>(LeanKitAccountAuth accountAuth, string resource, object body) where T : new();
		T Post<T>(LeanKitAccountAuth accountAuth, string resource) where T : new();
	}
}