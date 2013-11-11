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
	public class DuplicateItemException : Exception
	{
		#region Constructors

		/// <overloads>Initializes a new instance of the <see cref="DuplicateItemException" /> class.</overloads>
		/// <summary>
		///     Initializes a new instance of the <see cref="DuplicateItemException" /> class with the default error
		///     message.
		/// </summary>
		public DuplicateItemException()
			: this(null, null) { }

		/// <summary>
		///     Initializes a new instance of the <see cref="DuplicateItemException" /> class with a specified error
		///     message.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		public DuplicateItemException(string message)
			: this(message, null) { }

		/// <summary>
		///     Initializes a new instance of the <see cref="DuplicateItemException" /> class with the default error
		///     message and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="innerException">The exception that is the cause of the current exception.</param>
		public DuplicateItemException(Exception innerException)
			: this(null, innerException) { }

		/// <summary>
		///     Initializes a new instance of the <see cref="DuplicateItemException" /> class with a specified error
		///     message and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception.</param>
		public DuplicateItemException(string message, Exception innerException)
			: base(
				string.IsNullOrEmpty(message)
					? (innerException != null ? innerException.Message : "This action would result in a duplicate item.")
					: message, innerException) { }

		/// <summary>Initializes a new instance of the <see cref="DuplicateItemException" /> class with serialized data.</summary>
		/// <param name="info">The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination.</param>
		protected DuplicateItemException(SerializationInfo info, StreamingContext context)
			: base(info, context) { }

		#endregion
	}
}