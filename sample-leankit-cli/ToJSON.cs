using Newtonsoft.Json;

namespace leankit.cli
{
	public static class Helpers
	{
		public static string ToJSON(this object obj)
		{
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
		}
	}
}
