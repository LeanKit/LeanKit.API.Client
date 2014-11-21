//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using LeanKit.API.Client.Library.TransferObjects;

namespace LeanKit.API.Client.Library
{
	public class LeanKitIntegrationFactory : ILeanKitIntegrationFactory
	{
		/// <summary>
		///     Factory method that creates and returns the <see cref="LeanKitIntegration" /> implementation using the
		///     LeanKitClient created with the default <see cref="IRestCommandProcessor" />.
		/// </summary>
		/// <remarks>
		///     The default implementation of the <see cref="IRestCommandProcessor" /> uses RestSharp.
		/// </remarks>
		/// <param name="boardId">The Identity of the Board that will be watched and modified.</param>
		/// <param name="accountAuth">The account authentication information used to connect to the LeanKit API.</param>
		/// <returns>The <see cref="ILeanKitIntegration" /> used to monitor and modify the specified board. </returns>
		public ILeanKitIntegration Create(long boardId, ILeanKitAccountAuth accountAuth)
		{
			var clientFactory = new LeanKitClientFactory();
			var apiClient = clientFactory.Create(accountAuth);
			return new LeanKitIntegration(boardId, apiClient);
		}

		public ILeanKitIntegration Create(long boardId, ILeanKitAccountAuth accountAuth, IntegrationSettings settings)
		{
			var clientFactory = new LeanKitClientFactory();
			var apiClient = clientFactory.Create(accountAuth);
			return new LeanKitIntegration(boardId, apiClient, settings);
		}

		/// <summary>
		///     Factory method that creates and returns the <see cref="LeanKitIntegration" /> implementation using the
		///     LeanKitClient passed into the method.
		/// </summary>
		/// <remarks>
		///     This method should be used for consumers that would like to override the default
		///     <see cref="IRestCommandProcessor" />.
		/// </remarks>
		/// <param name="boardId">The Identity of the Board that will be watched and modified.</param>
		/// <param name="apiClient">The <see cref="ILeanKitApi" /> implementation used to communicate with the LeanKit API.</param>
		/// <returns></returns>
		public ILeanKitIntegration Create(long boardId, ILeanKitApi apiClient)
		{
			return new LeanKitIntegration(boardId, apiClient);
		}

		/// <summary>
		///     Factory method that creates and returns the <see cref="LeanKitIntegration" /> implementation using the
		///     LeanKitClient created with the default <see cref="IRestCommandProcessor" />.
		/// </summary>
		/// <remarks>
		///     The default implementation of the <see cref="IRestCommandProcessor" /> uses RestSharp.
		/// </remarks>
		/// <param name="boardId">The Identity of the Board that will be watched and modified.</param>
		/// <param name="accountAuth">The account authentication information used to connect to the LeanKit API.</param>
		/// <returns>The <see cref="ILeanKitIntegration" /> used to monitor and modify the specified board. </returns>
		[Obsolete("This is deprecated. Please use ILeanKitAccountAuth instead.")]
		public ILeanKitIntegration Create(long boardId, LeanKitAccountAuth accountAuth)
		{
			var clientFactory = new LeanKitClientFactory();
			var apiClient = clientFactory.Create(accountAuth);
			return new LeanKitIntegration(boardId, apiClient);
		}

		[Obsolete("This is deprecated. Please use ILeanKitAccountAuth instead.")]
		public ILeanKitIntegration Create(long boardId, LeanKitAccountAuth accountAuth, IntegrationSettings settings)
		{
			var clientFactory = new LeanKitClientFactory();
			var apiClient = clientFactory.Create(accountAuth);
			return new LeanKitIntegration(boardId, apiClient, settings);
		}

	}
}