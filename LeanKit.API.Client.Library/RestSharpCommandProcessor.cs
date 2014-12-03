//------------------------------------------------------------------------------
// <copyright company="LeanKit Inc.">
//     Copyright (c) LeanKit Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using LeanKit.API.Client.Library.Exceptions;
using LeanKit.API.Client.Library.TransferObjects;
using LeanKit.API.Client.Library.Validation;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;
using ErrorEventArgs = Newtonsoft.Json.Serialization.ErrorEventArgs;
using UnauthorizedAccessException = LeanKit.API.Client.Library.Exceptions.UnauthorizedAccessException;

namespace LeanKit.API.Client.Library
{
	public class RestSharpCommandProcessor : IRestCommandProcessor
	{
		private const string LeanKitUserAgent = "LeanKit.API.Client/1.0.8";
		private readonly IntegrationSettings _settings;
		private readonly IValidationService _validationService;

		public RestSharpCommandProcessor(IValidationService validationService, IntegrationSettings settings)
		{
			_validationService = validationService;
			_settings = settings;
		}

		public T Get<T>(ILeanKitAccountAuth accountAuth, string resource) where T : new()
		{
			return Process<T>(accountAuth,
				new RestRequest(Method.GET) { Resource = resource, RequestFormat = DataFormat.Json });
		}

		public T Get<T>(ILeanKitAccountAuth accountAuth, string resource, object body) where T : new()
		{
			var restRequest = new RestRequest(Method.GET) { Resource = resource, RequestFormat = DataFormat.Json };
			restRequest.AddBody(body);
			return Process<T>(accountAuth, restRequest);
		}

		public T Post<T>(ILeanKitAccountAuth accountAuth, string resource, object body) where T : new()
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

		public T Post<T>(ILeanKitAccountAuth accountAuth, string resource) where T : new()
		{
			return Process<T>(accountAuth,
				new RestRequest(Method.POST) { Resource = resource, RequestFormat = DataFormat.Json });
		}

		public T PostFile<T>(ILeanKitAccountAuth accountAuth, string resource, Dictionary<string, object> parameters, string fileName, string mimeType, byte[] fileBytes) where T : new()
		{
			var boundary = string.Format("----------{0:N}", Guid.NewGuid());
			var contentType = "multipart/form-data; boundary=" + boundary;
			var formBytes = GetMultipartFormData(parameters, boundary, fileName, mimeType, fileBytes);

			var request = WebRequest.Create(accountAuth.GetAccountUrl() + resource) as HttpWebRequest;

			if (request == null) throw new Exception("Error posting file. Could not create HttpWebRequest");

			request.Method = "POST";
			request.ContentType = contentType;
			request.ContentLength = formBytes.Length;
			request.UserAgent = LeanKitUserAgent;
			AddAuth(request, accountAuth);

			using (var requestStream = request.GetRequestStream())
			{
				requestStream.Write(formBytes, 0, formBytes.Length);
				requestStream.Close();
			}

			var response = request.GetResponse() as HttpWebResponse;
			var responseStream = new StreamReader(response.GetResponseStream());
			var result = responseStream.ReadToEnd();
			response.Close();

			//var task = PostFileAsync(accountAuth, resource, parameters, fileName, fileBytes);
			//var result = task.Result;

			if (string.IsNullOrEmpty(result)) return new T();
			var asyncResponse = JsonConvert.DeserializeObject<AsyncResponse2>(result);
			if (asyncResponse == null || asyncResponse.ReplyData == null || asyncResponse.ReplyData.Length == 0) return new T();

			var rawJson = asyncResponse.ReplyData[0].ToString();

			if (string.IsNullOrEmpty(rawJson)) return new T();

			var authUserDateFormat = _settings.DateFormat;

			var isoDateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = authUserDateFormat };

			var retVal = JsonConvert.DeserializeObject<T>(rawJson, new JsonSerializerSettings
			{
				Error = HandleError,
				Converters = { isoDateTimeConverter }
			});
			return retVal;
		}

		public AssetFile Download(ILeanKitAccountAuth accountAuth, string resource)
		{
			var request = WebRequest.Create(accountAuth.GetAccountUrl() + resource) as HttpWebRequest;

			if (request == null) throw new Exception("Error downloading file. Could not create HttpWebRequest");
			request.UserAgent = LeanKitUserAgent;
			AddAuth(request, accountAuth);

			var response = request.GetResponse() as HttpWebResponse;
			byte[] fileBytes;
			using (var responseStream = response.GetResponseStream())
			{
				var buffer = new byte[4096];
				using (var ms = new MemoryStream())
				{
					int read;
					while ((read = responseStream.Read(buffer, 0, buffer.Length)) > 0)
					{
						ms.Write(buffer, 0, read);
					}
					fileBytes = ms.ToArray();
				}
			}
			return new AssetFile
			{
				FileBytes = fileBytes,
				ContentType = response.ContentType,
				ContentLength = response.ContentLength
			};
		}

		private static byte[] GetMultipartFormData(Dictionary<string, object> parameters, string boundary, string fileName, string mimeType, byte[] fileBytes)
		{
			var encoding = Encoding.UTF8;
			var stream = new MemoryStream();
			var count = 0;
			foreach (var p in parameters)
			{
				if (count > 0)
					stream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));

				var data = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}", boundary, p.Key, p.Value);
				stream.Write(encoding.GetBytes(data), 0, encoding.GetByteCount(data));

				count++;
			}

			if (count > 0)
				stream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));

			if (string.IsNullOrEmpty(mimeType)) mimeType = "application/octet-stream";

			var header = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"file\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n", boundary, fileName, mimeType);
			stream.Write(encoding.GetBytes(header), 0, encoding.GetByteCount(header));
			stream.Write(fileBytes, 0, fileBytes.Length);

			var footer = string.Format("\r\n--{0}--\r\n", boundary);
			stream.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));

			stream.Position = 0;
			var formData = new byte[stream.Length];
			stream.Read(formData, 0, formData.Length);
			stream.Close();

			return formData;
		}

		private T Process<T>(ILeanKitAccountAuth accountAuth, IRestRequest request) where T : new()
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
				BaseUrl = new Uri(accountAuth.GetAccountUrl()),
				Authenticator = GetAuthenticator(accountAuth),
				UserAgent = LeanKitUserAgent
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

		private static IRestResponse<AsyncResponse> RetryRequest(IRestRequest request, ILeanKitAccountAuth accountAuth,
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
			ILeanKitAccountAuth leanKitAccountAuth,
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

		private static string FormatApiRequestInfo(ILeanKitAccountAuth leanKitAccountAuth, IRestRequest request, bool hidePassword = false)
		{
			var accountAuthStringStringBuilder = new StringBuilder();

			accountAuthStringStringBuilder.Append("Attempted Authentication Information: ");
			accountAuthStringStringBuilder.Append("Hostname: ");
			accountAuthStringStringBuilder.Append(leanKitAccountAuth.Hostname != null
				? leanKitAccountAuth.Hostname + ", "
				: "Unknown, ");
			
			var basicAuth = leanKitAccountAuth as LeanKitBasicAuth;
            if (basicAuth != null)
            {
                accountAuthStringStringBuilder.Append("Username: ");
                accountAuthStringStringBuilder.Append(basicAuth.Username != null
                    ? basicAuth.Username + ", "
                    : "Unknown, ");

                if (!hidePassword)
                {
                    accountAuthStringStringBuilder.Append("Password: ");
                    accountAuthStringStringBuilder.Append(basicAuth.Password != null
                        ? basicAuth.Password + ", "
                        : "Unknown, ");
                }
            }

            var tokenAuth = leanKitAccountAuth as LeanKitTokenAuth;
            if (tokenAuth != null && !hidePassword)
            {

                accountAuthStringStringBuilder.Append("Token: ");
                accountAuthStringStringBuilder.Append(tokenAuth.Token != null
                    ? tokenAuth.Token + ", "
                    : "Unknown, ");
            }
			accountAuthStringStringBuilder.Append("Resource: ");
			accountAuthStringStringBuilder.Append(request.Resource != null ? request.Resource + ", " : "Unknown, ");

			return accountAuthStringStringBuilder.ToString();
		}

		private static void AddAuth(WebRequest request, ILeanKitAccountAuth auth)
		{
			if (auth is LeanKitBasicAuth)
			{
				var basicAuth = auth as LeanKitBasicAuth;
				request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(basicAuth.Username + ":" + basicAuth.Password)));
			}
			else if (auth is LeanKitTokenAuth)
			{
				request.Headers.Add("Authorization", "Token " + (auth as LeanKitTokenAuth).Token);
			}
		}

		private static IAuthenticator GetAuthenticator(ILeanKitAccountAuth auth)
		{
			if (auth is LeanKitBasicAuth)
			{
				var basicAuth = auth as LeanKitBasicAuth;
				return new HttpBasicAuthenticator(basicAuth.Username, basicAuth.Password);
			}
			if (auth is LeanKitTokenAuth)
			{
				return new TokenAuthenticator((auth as LeanKitTokenAuth).Token);
			}
			throw new ArgumentException("Unknown ILeanKitAccountAuth type");
		}
	}
}