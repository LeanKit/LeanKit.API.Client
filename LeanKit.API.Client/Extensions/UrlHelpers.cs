using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace LeanKit.Extensions
{
    public static class UrlHelpers
    {
        public static Uri AttachParameters(this string uri, Dictionary<string, string> parameters)
        {
            if (uri == null || parameters == null || parameters.Count == 0)
            {
                return new Uri(uri, UriKind.RelativeOrAbsolute);
            }
            var stringBuilder = new StringBuilder();
            var str = "?";

            foreach (KeyValuePair<string, string> p in parameters)
            {
                stringBuilder.Append(str + WebUtility.UrlEncode(p.Key) + "=" + WebUtility.UrlEncode(p.Value));
                str = "&";
            }
            return new Uri(uri + stringBuilder, UriKind.RelativeOrAbsolute);
        }
    }
}
