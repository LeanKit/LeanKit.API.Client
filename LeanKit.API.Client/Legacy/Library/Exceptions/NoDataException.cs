using System;
using System.Runtime.Serialization;
using LeanKit.API.Legacy.Library.TransferObjects;

namespace LeanKit.API.Legacy.Library.Exceptions
{
	[Serializable]
	public class NoDataException : LeanKitAPIException
	{
		#region Constructors

		public NoDataException() : this(null, null) { }

		public NoDataException(string message) : this(message, null) { }

		public NoDataException(Exception innerException) : this(null, innerException) { }


		public NoDataException(string message, Exception innerException)
			: base(
				string.IsNullOrEmpty(message)
					? (innerException != null ? innerException.Message : "No data was found for the respective call.")
					: message, ResponseCode.NoData, innerException) { }

		protected NoDataException(SerializationInfo info, StreamingContext context) : base(info, context) { }

		#endregion
	}
}