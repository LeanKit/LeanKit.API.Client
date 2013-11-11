//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Runtime.Serialization;

namespace LeanKit.API.Client.Library.Exceptions
{
	[Serializable]
	public class ItemNotFoundException : Exception
	{
		#region Constructors

		/// <overloads>Initializes a new instance of the <see cref="ItemNotFoundException" /> class.</overloads>
		/// <summary>
		///     Initializes a new instance of the <see cref="ItemNotFoundException" /> class with the default error
		///     message.
		/// </summary>
		public ItemNotFoundException() : this(null, null) { }

		/// <summary>
		///     Initializes a new instance of the <see cref="ItemNotFoundException" /> class with a specified error
		///     message.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		public ItemNotFoundException(string message) : this(message, null) { }

		/// <summary>
		///     Initializes a new instance of the <see cref="ItemNotFoundException" /> class with the default error
		///     message and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="innerException">The exception that is the cause of the current exception.</param>
		public ItemNotFoundException(Exception innerException) : this(null, innerException) { }

		/// <summary>
		///     Initializes a new instance of the <see cref="ItemNotFoundException" /> class with a specified error
		///     message and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception.</param>
		public ItemNotFoundException(string message, Exception innerException)
			: base(
				string.IsNullOrEmpty(message)
					? (innerException != null
						? innerException.Message
						: "The specified item could not be located.  Please check the item identifier.")
					: message, innerException) { }

		/// <summary>Initializes a new instance of the <see cref="ItemNotFoundException" /> class with serialized data.</summary>
		/// <param name="info">The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination.</param>
		protected ItemNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }

		#endregion
	}
}