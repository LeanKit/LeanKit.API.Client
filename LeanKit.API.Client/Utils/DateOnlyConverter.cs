using System;
using Newtonsoft.Json.Converters;

namespace LeanKit.Utils
{
    public class DateOnlyConverter : IsoDateTimeConverter
    {
        public DateOnlyConverter()
        {
            base.DateTimeFormat = "yyyy-MM-dd";
        }
    }
}