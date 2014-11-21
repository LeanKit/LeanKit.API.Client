using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace LeanKit.API.Client.Library
{
	public class LeanKitAccountAuthBase : ILeanKitAccountAuth
	{
		public LeanKitAccountAuthBase()
        {
            UrlTemplateOverride = ConfigurationManager.AppSettings["UrlTemplateOverride"] ?? "https://{0}.leankit.com";
        }

        public string Hostname { get; set; }
        public string UrlTemplateOverride { get; set; }

        public string GetAccountUrl()
        {
            return string.Format(UrlTemplateOverride, Hostname);
        }
	}
}
