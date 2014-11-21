//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using LeanKit.API.Client.Library.TransferObjects;
using RestSharp;

namespace LeanKit.API.Client.Library
{
	/// <summary>
	///     The IRestCommandProcessor is responsible for communicating via HTTP with the LeanKit API server on behalf of the
	///     classes that consume it.
	/// </summary>
	public interface IRestCommandProcessor
	{
		T Get<T>(ILeanKitAccountAuth accountAuth, string resource) where T : new();
		T Get<T>(ILeanKitAccountAuth accountAuth, string resource, object body) where T : new();
		T Post<T>(ILeanKitAccountAuth accountAuth, string resource, object body) where T : new();
		T Post<T>(ILeanKitAccountAuth accountAuth, string resource) where T : new();
		T PostFile<T>(ILeanKitAccountAuth accountAuth, string resource, Dictionary<string, object> parameters, string fileName, string mimeType, byte[] fileBytes) where T : new();
	}
}