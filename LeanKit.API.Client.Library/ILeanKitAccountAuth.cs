using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeanKit.API.Client.Library
{
	public interface ILeanKitAccountAuth
	{
		string GetAccountUrl();
		string Hostname { get; set; }
	}
}
