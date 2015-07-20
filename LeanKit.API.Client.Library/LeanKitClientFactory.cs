//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using LeanKit.API.Client.Library.TransferObjects;
using LeanKit.API.Client.Library.Validation;

namespace LeanKit.API.Client.Library
{
	public class LeanKitClientFactory : ILeanKitClientFactory
	{
		[Obsolete("This is deprecated. Please use ILeanKitAccountAuth instead.")]
		public ILeanKitApi Create(LeanKitAccountAuth accountAuth)
		{
			var leanKitClient = new LeanKitClient(new RestSharpCommandProcessor(new ValidationService(null), new IntegrationSettings()));
			return leanKitClient.Initialize(accountAuth);
		}

		public ILeanKitApi Create(ILeanKitAccountAuth accountAuth)
		{
			var leanKitClient = new LeanKitClient(new RestSharpCommandProcessor(new ValidationService(null), new IntegrationSettings()));
			return leanKitClient.Initialize(accountAuth);			
		}

		public ILeanKitApi Create(ILeanKitAccountAuth accountAuth, string dateFormat)
		{
			var settings = new IntegrationSettings() {DateFormat = dateFormat};
			var leanKitClient = new LeanKitClient(new RestSharpCommandProcessor(new ValidationService(null), settings));
			return leanKitClient.Initialize(accountAuth);
		}

	}
}