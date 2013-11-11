//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Runtime.Serialization;
using LeanKit.API.Client.Library.TransferObjects;

namespace LeanKit.API.Client.Library.Exceptions
{
	[Serializable]
	public class InvalidAPIResourceException : LeanKitAPIException
	{
		public InvalidAPIResourceException() : this(null, null) { }

		public InvalidAPIResourceException(string message) : this(message, null) { }

		public InvalidAPIResourceException(Exception innerException) : this(null, innerException) { }

		public InvalidAPIResourceException(string message, Exception innerException)
			: base(
				string.IsNullOrEmpty(message)
					? (innerException != null
						? innerException.Message
						: "The specified API resource is not valid.  Please check the provide HostName and overriding URL.")
					: message, ResponseCode.None, innerException) { }

		protected InvalidAPIResourceException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}