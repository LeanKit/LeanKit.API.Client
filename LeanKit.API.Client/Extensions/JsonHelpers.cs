using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LeanKit.Extensions
{
	public static class JsonHelpers
	{
		public static string ToJSON(this object obj)
		{
            return JsonConvert.SerializeObject(obj, Formatting.None, 
                                               new JsonSerializerSettings 
            { 
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            } );
		}
	}
}
