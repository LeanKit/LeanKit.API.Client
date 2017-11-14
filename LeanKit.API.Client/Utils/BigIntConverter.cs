using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LeanKit.Utils
{
    public class BigIntConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
        public override bool CanRead
        {
            get
            {
                return false;
            }
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

	public class BigIntArrayConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return true;
		}
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
            if (value == null) return;
            writer.WriteStartArray();
            var longs = value as IEnumerable<long>;
            foreach( var l in longs)
            {
                writer.WriteValue(l.ToString());
            }
			writer.WriteEndArray();
		}
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}
	}
}
