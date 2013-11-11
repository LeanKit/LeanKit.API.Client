//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using LeanKit.API.Client.Library.TransferObjects;
using RestSharp;

namespace LeanKit.API.Client.Library.Validation
{
	public interface IValidationService
	{
		List<ValidationResult> ValidateRequest(RestRequest request);
	}

	public class ValidationService : IValidationService
	{
		private const string EntityIdQualifier = @".*id";
		private const string EntityIdsQualifier = @".*ids";
		private const string ApiTypesNamespaceQualifier = "LeanKit.API.Client.Library.TransferObjects";

		private readonly Board _board;
		private readonly List<ValidationResult> _validationResults;

		public ValidationService(Board board)
		{
			_board = board;
			_validationResults = new List<ValidationResult>();
		}

		public List<ValidationResult> ValidateRequest(RestRequest request)
		{
			foreach (Parameter requestParameter in request.Parameters)
			{
				string pName = requestParameter.Name;
				object pValue = requestParameter.Value;
				if (pValue == null)
				{
					continue;
				}

				Type pType = pValue.GetType();

				if (pType.IsPrimitive)
				{
					TryValidateDomainIdentity(pName, EntityIdQualifier, pValue, pType);
				}
				else if (pType.IsClass)
				{
					if (typeof (IEnumerable).IsAssignableFrom(pType) && pType.IsGenericType)
					{
						Type genericType = pType.GetGenericArguments()[0];
						if (genericType.IsPrimitive)
						{
							TryValidateDomainIdentity(pName, EntityIdsQualifier, pValue, pType);
						}
						else if (genericType.IsClass)
						{
							List<object> pValues = ((IEnumerable) pValue).Cast<object>().ToList();
							foreach (object innerValue in pValues)
							{
								string newName = string.Format("{0}[{1}]", pName, pValues.IndexOf(innerValue));
								ValidateDomainEntity(newName, innerValue, innerValue.GetType());
							}
						}
					}
					else
					{
						ValidateDomainEntity(pName, pValue, pType);
					}
				}
			}

			return _validationResults;
		}

		private void ValidateDomainEntity(string parameterName, object parameterValue, Type parameterType)
		{
			PropertyInfo[] allProperties = parameterType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

			//perform DataAnnotations validation
			if (allProperties.Any(x => x.GetCustomAttributes(typeof (ValidationAttribute), true).Length > 0))
			{
				var classValidatonResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
				bool isValid = Validator.TryValidateObject(parameterValue,
					new ValidationContext(parameterValue, null, null),
					classValidatonResults);
				if (!isValid)
				{
					_validationResults.AddRange(
						classValidatonResults.Select(x => new ValidationResult(parameterName, x)));
				}
			}

			//perform custom validation
			IEnumerable<PropertyInfo> customPropertiesInfo =
				allProperties.Where(x => x.GetCustomAttributes(typeof (DomainIdentityValidationAttribute), false).Length > 0);
			foreach (PropertyInfo customPropertyInfo in customPropertiesInfo)
			{
				Type customPropertyType = customPropertyInfo.PropertyType;
				object customPropertyValue = customPropertyInfo.GetValue(parameterValue,
					BindingFlags.Public | BindingFlags.Instance,
					null, null, CultureInfo.InvariantCulture);

				long[] domainIdentities = GetDomainIdentities(customPropertyValue, customPropertyType);
				if (domainIdentities != null)
				{
					foreach (long domainIdentity in domainIdentities)
					{
						ValidateDomainIdentity(customPropertyInfo.Name, domainIdentity,
							string.Format("{0}.{1}", parameterName, customPropertyInfo.Name));
					}
				}
			}

			//drill through nested properties and try to validate them
			foreach (
				PropertyInfo nestedProperty in allProperties.Where(x => x.PropertyType.IsClass || x.PropertyType.IsInterface))
			{
				string npName = nestedProperty.Name;
				Type npType = nestedProperty.PropertyType;
				object npValue = nestedProperty.GetValue(parameterValue, BindingFlags.Public | BindingFlags.Instance, null, null,
					CultureInfo.InvariantCulture);

				if (npValue == null)
				{
					continue;
				}

				if (typeof (IEnumerable).IsAssignableFrom(npType) && npType.IsGenericType)
				{
					Type genericType = npType.GetGenericArguments()[0];
					if (ApiTypesNamespaceQualifier.Equals(genericType.Namespace, StringComparison.Ordinal))
					{
						List<object> pValues = ((IEnumerable) npValue).Cast<object>().ToList();
						foreach (object innerValue in pValues)
						{
							string newName = string.Format("{0}.{1}[{2}]", parameterName, npName, pValues.IndexOf(innerValue));
							ValidateDomainEntity(newName, innerValue, innerValue.GetType());
						}
					}
				}
				else
				{
					if (ApiTypesNamespaceQualifier.Equals(npType.Namespace, StringComparison.Ordinal))
					{
						string newName = string.Format("{0}.{1}", parameterName, npName);
						ValidateDomainEntity(newName, npValue, npType);
					}
				}
			}
		}

		private void TryValidateDomainIdentity(string parameterName, string matchPattern, object parameterValue,
			Type parameterType)
		{
			bool nameMatched = Regex.IsMatch(parameterName, matchPattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);
			if (nameMatched)
			{
				long[] domainIdentities = GetDomainIdentities(parameterValue, parameterType);
				if (domainIdentities != null)
				{
					Array.ForEach(domainIdentities, x =>
					{
						if (domainIdentities.Length > 1)
						{
							string correctedParameterName = string.Format("{0}[{1}]", parameterName, Array.IndexOf(domainIdentities, x));
							ValidateDomainIdentity(parameterName.Substring(0, parameterName.Length - 1), x, correctedParameterName);
						}
						else
						{
							ValidateDomainIdentity(parameterName, x);
						}
					});
				}
			}
		}

		private void ValidateDomainIdentity(string parameterName, long parameterValue, string correctedParameterName = null)
		{
			string newParameterName = !string.IsNullOrEmpty(correctedParameterName)
				? correctedParameterName
				: parameterName;
			if (parameterValue <= 0)
			{
				_validationResults.Add(GetValidationResult(newParameterName, parameterValue,
					"Domain identity parameter must be greater than zero"));
			}

			if (_board != null)
			{
				object entity;
				switch (parameterName.ToLowerInvariant())
				{
					case "cardid":
						entity = _board.Lanes.SelectMany(x => x.Cards).FirstOrDefault(x => x.Id == parameterValue);
						break;
					case "laneid":
					case "tolaneid":
						entity = _board.Lanes.FirstOrDefault(x => x.Id == parameterValue);
						break;
					case "typeid":
						entity = _board.CardTypes.FirstOrDefault(x => x.Id == parameterValue);
						break;
					case "classofserviceid":
						entity = _board.ClassesOfService.FirstOrDefault(x => x.Id == parameterValue);
						break;
					case "assigneduserids":
						entity = _board.BoardUsers.FirstOrDefault(x => x.Id == parameterValue);
						break;
					default:
						return;
				}

				if (entity == null)
				{
					_validationResults.Add(GetValidationResult(newParameterName, parameterValue,
						"Domain identity parameter doesn't have corresponding entity"));
				}
			}
		}

		private long[] GetDomainIdentities(object value, Type type)
		{
			if (typeof (IEnumerable).IsAssignableFrom(type))
			{
				if (value != null) return (value as IEnumerable).Cast<long>().ToArray();
			}

			var domainIdentities = new[] {Convert.ToInt64(value)};
			return domainIdentities;
		}

		private ValidationResult GetValidationResult(string parameterName, object parameterValue, string message)
		{
			return new ValidationResult(string.Format("Name = {0}, Value = {1}. {2}", parameterName, parameterValue, message));
		}
	}
}