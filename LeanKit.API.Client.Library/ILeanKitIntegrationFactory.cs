//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using LeanKit.API.Client.Library.TransferObjects;

namespace LeanKit.API.Client.Library
{
	public interface ILeanKitIntegrationFactory
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
		[Obsolete("This is deprecated. Please use ILeanKitAccountAuth instead.")]
		ILeanKitIntegration Create(long boardId, LeanKitAccountAuth accountAuth);
		[Obsolete("This is deprecated. Please use ILeanKitAccountAuth instead.")]
		ILeanKitIntegration Create(long boardId, LeanKitAccountAuth accountAuth, IntegrationSettings settings);

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
		ILeanKitIntegration Create(long boardId, ILeanKitAccountAuth accountAuth);
		[Obsolete("This is deprecated. Please use ILeanKitAccountAuth instead.")]
		ILeanKitIntegration Create(long boardId, ILeanKitAccountAuth accountAuth, IntegrationSettings settings);
	}
}