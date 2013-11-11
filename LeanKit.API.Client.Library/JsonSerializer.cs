//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Newtonsoft.Json;
using RestSharp.Serializers;

namespace LeanKit.API.Client.Library
{
	/// <summary>
	///     This implementation of ISerializer exists to replace the JsonSerializer that
	///     used to be a part of the RestSharp library. It is a temporary fix until
	///     the RestSharp team adds it back into their source code.
	/// </summary>
	public class JsonSerializer : ISerializer
	{
		/// <summary>
		///     Default serializer
		/// </summary>
		public JsonSerializer()
		{
			ContentType = "application/json";
		}

		/// <summary>
		///     Serialize the object as JSON
		/// </summary>
		/// <param name="obj">Object to serialize</param>
		/// <returns>JSON as String</returns>
		public string Serialize(object obj)
		{
			return JsonConvert.SerializeObject(obj);
		}

		public string DateFormat { get; set; }
		public string RootElement { get; set; }
		public string Namespace { get; set; }
		public string ContentType { get; set; }
	}
}