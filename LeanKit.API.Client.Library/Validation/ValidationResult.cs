//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Linq;

namespace LeanKit.API.Client.Library.Validation
{
	public class ValidationResult : System.ComponentModel.DataAnnotations.ValidationResult
	{
		public ValidationResult(string errorMessage) : base(errorMessage)
		{
		}

		public ValidationResult(string prefix, System.ComponentModel.DataAnnotations.ValidationResult value) : base(value)
		{
			Prefix = prefix;
		}

		public string Prefix { get; private set; }

		public override string ToString()
		{
			if (MemberNames.Any())
			{
				string membersNames = string.Join(",", !string.IsNullOrEmpty(Prefix)
					? MemberNames.Select(x => string.Format("{0}.{1}", Prefix, x))
					: MemberNames);
				return string.Format("Name = {0}. {1}", membersNames, ErrorMessage);
			}
			return ErrorMessage;
		}
	}
}