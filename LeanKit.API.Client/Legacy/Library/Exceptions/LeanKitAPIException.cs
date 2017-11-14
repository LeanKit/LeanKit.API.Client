using System;
using System.Runtime.Serialization;
using LeanKit.API.Legacy.Library.TransferObjects;

namespace LeanKit.API.Legacy.Library.Exceptions
{
	[Serializable]
	public class LeanKitAPIException : Exception
	{
		#region Constructors

		public LeanKitAPIException() : this(null, ResponseCode.None, null) { }

		public LeanKitAPIException(string message) : this(message, ResponseCode.None, null) { }

		public LeanKitAPIException(string message, ResponseCode responseCode) : this(message, responseCode, null) { }

		public LeanKitAPIException(ResponseCode responseCode, Exception innerException) : this(null, responseCode, innerException) { }

		public LeanKitAPIException(Exception innerException) : this(null, ResponseCode.None, innerException) { }

		public LeanKitAPIException(string message, ResponseCode responseCode, Exception innerException)
			: base(
				string.IsNullOrEmpty(message)
					? (innerException != null
						? innerException.Message
						: "A problem occurred while communicating with the LeanKit API or during the processing of the data.")
					: message, innerException) { ResponseCode = responseCode; }

		protected LeanKitAPIException(SerializationInfo info, StreamingContext context) : base(info, context) { }

		#endregion

		public ResponseCode ResponseCode { get; set; }
	}
}