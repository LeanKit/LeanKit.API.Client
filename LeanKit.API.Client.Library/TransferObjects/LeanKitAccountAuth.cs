//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Configuration;

namespace LeanKit.API.Client.Library.TransferObjects
{
	public class LeanKitAccountAuth
	{
		public LeanKitAccountAuth()
		{
			UrlTemplateOverride = ConfigurationManager.AppSettings["UrlTemplateOverride"] ?? "https://{0}.leankit.com";
		}

		public string Username { get; set; }

		public string Password { get; set; }

		public string Hostname { get; set; }

		public string UrlTemplateOverride { get; set; }

		public string GetAccountUrl()
		{
			return string.Format(UrlTemplateOverride, Hostname);
		}
	}
}