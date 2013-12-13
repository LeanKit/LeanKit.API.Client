//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using System.Threading;
using LeanKit.API.Client.Library.Exceptions;
using LeanKit.API.Client.Library.TransferObjects;
using LeanKit.API.Client.Library.Validation;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using RestSharp;
using UnauthorizedAccessException = LeanKit.API.Client.Library.Exceptions.UnauthorizedAccessException;

namespace LeanKit.API.Client.Library
{
	public class RestSharpCommandProcessor : IRestCommandProcessor
	{
		private readonly IntegrationSettings _settings;
		private readonly IValidationService _validationService;

		public RestSharpCommandProcessor(IValidationService validationService, IntegrationSettings settings)
		{
			_validationService = validationService;
			_settings = settings;
		}

		public T Get<T>(LeanKitAccountAuth accountAuth, string resource) where T : new()
		{
			return Process<T>(accountAuth,
				new RestRequest(Method.GET) {Resource = resource, RequestFormat = DataFormat.Json});
		}

		public T Get<T>(LeanKitAccountAuth accountAuth, string resource, object body) where T : new()
		{
			var restRequest = new RestRequest(Method.GET) {Resource = resource, RequestFormat = DataFormat.Json};
			restRequest.AddBody(body);
			return Process<T>(accountAuth, restRequest);
		}

		public T Post<T>(LeanKitAccountAuth accountAuth, string resource, object body) where T : new()
		{
			var restRequest = new RestRequest(Method.POST)
			{
				Resource = resource,
				RequestFormat = DataFormat.Json,
				JsonSerializer = new JsonSerializer()
			};
			restRequest.AddBody(body);
			return Process<T>(accountAuth, restRequest);
		}

		public T Post<T>(LeanKitAccountAuth accountAuth, string resource) where T : new()
		{
			return Process<T>(accountAuth,
				new RestRequest(Method.POST) {Resource = resource, RequestFormat = DataFormat.Json});
		}

		private T Process<T>(LeanKitAccountAuth accountAuth, IRestRequest request) where T : new()
		{
			var errorMessages = _validationService.ValidateRequest((RestRequest) request);
			if (errorMessages.Count > 0)
			{
				var ex = new ValidationException("Provided request parameters are invalid.");
				ex.Data["ErrorMessages"] = errorMessages;
				throw ex;
			}
			var client = new RestClient
			{
				BaseUrl = accountAuth.GetAccountUrl(),
				Authenticator = new HttpBasicAuthenticator(accountAuth.Username, accountAuth.Password)
			};

			var response = RetryRequest(request, accountAuth, client);

			var asyncResponse = response.Data;
			if (string.IsNullOrEmpty(asyncResponse.ReplyData))
			{
				return new T();
			}
			var rawJson = asyncResponse.ReplyData;

			var authUserDateFormat = _settings.DateFormat;

			var isoDateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = authUserDateFormat };

			rawJson = rawJson.Substring(1, rawJson.Length - 2);

			var retVal = JsonConvert.DeserializeObject<T>(rawJson, new JsonSerializerSettings
			{
				Error = HandleError,
				Converters = {isoDateTimeConverter}
			});
			return retVal;
		}

		private static IRestResponse<AsyncResponse> RetryRequest(IRestRequest request, LeanKitAccountAuth accountAuth,
			RestClient client)
		{
			IRestResponse<AsyncResponse> response = new RestResponse<AsyncResponse>();
			for (var tryCount = 0; tryCount <= 10; tryCount++)
			{
				try
				{
					response = client.Execute<AsyncResponse>(request);
					ValidateResponse(response, accountAuth, request);
					break;
				}
				catch (NoDataException)
				{
					return response;
				}
				catch (Exception)
				{
					if (tryCount == 10) throw;
					Thread.Sleep(1000);
				}
			}
			return response;
		}

		private static void HandleError(object sender, ErrorEventArgs e)
		{
			e.ErrorContext.Handled = true;
		}

		private static Uri GetRequestUri(string fullUrl)
		{
			return new Uri(fullUrl);
		}

		private static void ValidateResponse(IRestResponse<AsyncResponse> response,
			LeanKitAccountAuth leanKitAccountAuth,
			IRestRequest request)
		{
			var requestedResource = GetRequestUri(leanKitAccountAuth.GetAccountUrl());

			var logAuthString = FormatApiRequestInfo(leanKitAccountAuth, request, true);

			if (response == null)
			{
				throw new LeanKitAPIException("A failure occurred retrieving the response from the API.");
			}

			if (response.ResponseUri.Host != requestedResource.Host)
			{
				throw new InvalidAPIResourceException();
			}

			if (response.ResponseStatus == ResponseStatus.Error)
			{
				throw new LeanKitAPIException(response.ErrorException);
			}

			switch (response.StatusCode)
			{
				case HttpStatusCode.Unauthorized:
					throw new UnauthorizedAccessException(response.StatusDescription + ": " + logAuthString);
			}

			AsyncResponse responseData = response.Data;
			if (responseData == null)
			{
				throw new LeanKitAPIException("Unable to process the response from the API.");
			}

			switch ((ResponseCode) responseData.ReplyCode)
			{
				case ResponseCode.UnauthorizedAccess:
					throw new UnauthorizedAccessException(responseData.ReplyText + ": " + logAuthString);
				case ResponseCode.NoData:
					throw new NoDataException(responseData.ReplyText);
				case ResponseCode.FatalException:
				case ResponseCode.MinorException:
				case ResponseCode.UserException:
					throw new LeanKitAPIException(responseData.ReplyText, (ResponseCode) responseData.ReplyCode);
			}

			//check to make sure there is data
			if (string.IsNullOrEmpty(responseData.ReplyData))
			{
				throw new LeanKitAPIException(responseData.ReplyText + ": " + logAuthString, (ResponseCode) responseData.ReplyCode);
			}
		}

		private static string FormatApiRequestInfo(LeanKitAccountAuth leanKitAccountAuth, IRestRequest request, bool hidePassword = false)
		{
			var accountAuthStringStringBuilder = new StringBuilder();

			accountAuthStringStringBuilder.Append("Attempted Authentication Information: ");
			accountAuthStringStringBuilder.Append("Hostname: ");
			accountAuthStringStringBuilder.Append(leanKitAccountAuth.Hostname != null
				? leanKitAccountAuth.Hostname + ", "
				: "Unknown, ");
			accountAuthStringStringBuilder.Append("Username: ");
			accountAuthStringStringBuilder.Append(leanKitAccountAuth.Username != null
				? leanKitAccountAuth.Username + ", "
				: "Unknown, ");

			if (!hidePassword)
			{
				accountAuthStringStringBuilder.Append("Password: ");
				accountAuthStringStringBuilder.Append(leanKitAccountAuth.Password != null
					? leanKitAccountAuth.Password + ", "
					: "Unknown, ");
			}
			accountAuthStringStringBuilder.Append("Resource: ");
			accountAuthStringStringBuilder.Append(request.Resource != null ? request.Resource + ", " : "Unknown, ");

			return accountAuthStringStringBuilder.ToString();
		}
	}
}