//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using LeanKit.API.Client.Library.TransferObjects;
using LeanKit.API.Client.Library.Validation;

namespace LeanKit.API.Client.Library
{
	public class LeanKitClientFactory : ILeanKitClientFactory
	{
		public ILeanKitApi Create(LeanKitAccountAuth accountAuth)
		{
			var leanKitClient = new LeanKitClient(new RestSharpCommandProcessor(new ValidationService(null), new IntegrationSettings()));
			return leanKitClient.Initialize(accountAuth);
		}
	}
}