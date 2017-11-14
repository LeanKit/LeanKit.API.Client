using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LeanKit.API.Legacy.Library.TransferObjects;
using LeanKit.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LeanKit.API
{
    public class Client
    {
        private string _baseUrl;
        private string _userName;
        private string _password;
        private string _token;
        private readonly Board _board;
        private readonly Card _card;
        private readonly Auth _auth;
        private readonly Account _account;
        private readonly Template _template;
        private readonly User _user;
        private readonly TaskCard _taskCard;
        private readonly Legacy.Library.LegacyApi _legacyApi;
        private static readonly HttpClient _client = new HttpClient();

        private Client() 
        {
			_board = new Board(this);
			_card = new Card(this);
            _auth = new Auth(this);
            _account = new Account(this);
            _template = new Template(this);
            _user = new User(this);
            _taskCard = new TaskCard(this);
            _legacyApi = new Legacy.Library.LegacyApi(this);
		}

        public Client(string hostName, string userName, string password, string dateFormat = "MM/dd/yyyy") : this()
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hostName))
            {
                throw new ArgumentException("A valid hostName, userName, and password are required.");
            }
            this.BaseUrl = hostName;
            _userName = userName;
            _password = password;
			this.DateFormat = dateFormat;
			Initialize();
        }

        public Client(string hostName, string token, string dateFormat = "MM/dd/yyyy"): this()
        {
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(hostName))
            {
                throw new ArgumentException("A valid hostName and token are required.");
            }
            this.BaseUrl = hostName;
            _token = token;
			this.DateFormat = dateFormat;
			Initialize();
        }

        private void Initialize()
        {
            // TODO: Better way to maintain version numbers, and support pre-release versions
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            _client.BaseAddress = new Uri(BaseUrl);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.UserAgent.ParseAdd($"LeanKit.API.Client/{version}-alpha");
            if (!string.IsNullOrWhiteSpace(_userName))
            {
                var authByteArray = Encoding.ASCII.GetBytes(string.Format("{0}:{1}", _userName, _password));
                var authString = Convert.ToBase64String(authByteArray);
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authString);
            }
            else if (!string.IsNullOrWhiteSpace(_token))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            }
        }

        public string BaseUrl
        {
            get { return _baseUrl; }
            private set
            {
                _baseUrl = value.StartsWith("http", StringComparison.OrdinalIgnoreCase) ? value : string.Format("https://{0}.leankit.com", value);
            }
        }

        public string DateFormat { get; set; }

        public Account Account
        {
            get { return _account; }
        }

        public Auth Auth
        {
            get { return _auth; }
        }

        public Board Board
        {
            get { return _board; }
        }

        public Card Card
        {
            get { return _card; }
        }

        public Template Template
        {
            get { return _template; }
        }

        public User User
        {
            get { return _user; }
        }

        public TaskCard TaskCard
        {
            get { return _taskCard; }
        }

        public Legacy.Library.LegacyApi Legacy
        {
            get { return _legacyApi; }
        }

        internal async Task<T> Request<T>(string path, string method = "get", Dictionary<string, string> parameters = null, string body = null)
        {
            var resource = path.AttachParameters(parameters);
			// TODO: debug logging
			Console.WriteLine(resource);
            HttpResponseMessage response = null;
            HttpContent content = null;
            if (method != "get" && !string.IsNullOrEmpty(body))
            {
				// TODO: debug logging
				Console.WriteLine("Body: " + body);
                content = new StringContent(body, Encoding.UTF8, "application/json");
            }
            switch (method.ToLowerInvariant())
            {
                case "get":
                    var res = await _client.GetStringAsync(resource);
                    return JsonConvert.DeserializeObject<T>(res);
                case "post":
                    response = await _client.PostAsync(resource, content);
                    break;
                case "put":
                    response = await _client.PutAsync(resource, content);
                    break;
                case "patch":
                    response = await _client.PatchAsync(resource, content);
                    break;
                case "delete":
                    response = await _client.DeleteAsync(resource);
                    break;
                default:
                    return default(T);
            }
            return await ParseResponse<T>(response);
        }

        internal async Task<T> LegacyRequest<T>(string path, string method="get", string body = null)
        {
			// TODO: debug logging
			Console.WriteLine(path);
			HttpResponseMessage response = null;
			HttpContent content = null;
			if (method != "get" && !string.IsNullOrEmpty(body))
			{
				// TODO: debug logging
				Console.WriteLine("Body: " + body);
				content = new StringContent(body, Encoding.UTF8, "application/json");
			}
			switch (method.ToLowerInvariant())
			{
				case "get":
                    response = await _client.GetAsync(path);
                    break;
				case "post":
					response = await _client.PostAsync(path, content);
					break;
				default:
					return default(T);
			}
			return await ParseLegacyResponse<T>(response);
        }

		internal async Task<T> PostFile<T>(string path, Dictionary<string, string> formValues, string name, byte[] fileBytes)
		{
			var form = new MultipartFormDataContent();

			var fileContent = new ByteArrayContent(fileBytes);
			fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
			form.Add(fileContent, "File", name);
			foreach (var formValue in formValues)
			{
				form.Add(new StringContent(formValue.Value), formValue.Key);
			}
			var response = await _client.PostAsync(path, form);
			return await ParseResponse<T>(response);
		}

        internal async Task<Models.AttachmentFileResponse> GetFile(string path)
        {
            var attachment = new Models.AttachmentFileResponse();
            var response = await _client.GetAsync(path, HttpCompletionOption.ResponseHeadersRead);
			response.EnsureSuccessStatusCode();
			attachment.FileBytes = await response.Content.ReadAsByteArrayAsync();
            attachment.ContentType = response.Content.Headers.ContentType.MediaType;
            attachment.ContentLength = response.Content.Headers.ContentLength.Value;
            return attachment;
        }

		internal async Task<T> LegacyPostFile<T>(string path, Dictionary<string, string> formValues, string name, byte[] fileBytes)
		{
			var form = new MultipartFormDataContent();

			var fileContent = new ByteArrayContent(fileBytes);
			fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
			form.Add(fileContent, "File", name);
			foreach (var formValue in formValues)
			{
				form.Add(new StringContent(formValue.Value), formValue.Key);
			}
			var response = await _client.PostAsync(path, form);
			return await ParseLegacyResponse<T>(response);
		}

		internal async Task<AssetFile> LegacyGetFile(string path)
		{
			var attachment = new AssetFile();
			var response = await _client.GetAsync(path, HttpCompletionOption.ResponseHeadersRead);
			response.EnsureSuccessStatusCode();
			attachment.FileBytes = await response.Content.ReadAsByteArrayAsync();
			attachment.ContentType = response.Content.Headers.ContentType.MediaType;
			attachment.ContentLength = response.Content.Headers.ContentLength.Value;
			return attachment;
		}

		private async Task<T> ParseResponse<T>(HttpResponseMessage response)
        {
			response.EnsureSuccessStatusCode();
			if (response.Content != null)
			{
				var str = await response.Content.ReadAsStringAsync();
				if (!string.IsNullOrEmpty(str))
				{
                    // TODO: debug logging
					Console.WriteLine("Response Content: " + str);
					return JsonConvert.DeserializeObject<T>(str);
				}
			}
			return default(T);
		}

        private async Task<T> ParseLegacyResponse<T>(HttpResponseMessage response)
        {
			response.EnsureSuccessStatusCode();
			if (response.Content != null)
			{
				var str = await response.Content.ReadAsStringAsync();
				if (!string.IsNullOrEmpty(str))
				{
					// TODO: debug logging
					Console.WriteLine("Response Content: " + str);
					var authUserDateFormat = this.DateFormat + " hh:mm:ss tt";
                    var serialzationSettings = new JsonSerializerSettings
                    {
                        Error = HandleError,
                        DateFormatString = authUserDateFormat
                    };

					try
                    {
                        var res = JsonConvert.DeserializeObject<LegacyResponse>(str);
                        if (res.ReplyData == null || res.ReplyData.Length == 0 || res.ReplyData[0] == null)
                        {
                            return default(T);
                        }
                        var rawJson = res.ReplyData[0].ToString();
                        if (res.ReplyCode >= 300)
                        {
                            throw new Exception(string.Format("API Error: {0}, {1}", res.ReplyCode, res.ReplyText));
                        }
                        return JsonConvert.DeserializeObject<T>(rawJson, serialzationSettings);
                    } 
                    catch (JsonSerializationException)
                    {
						// Some endpoints, such as /Kanban/API/ListNewBoards, return an unwrapped response
						return JsonConvert.DeserializeObject<T>(str, serialzationSettings);
					}
                    catch (Exception ex)
                    {
                        throw ex;
                    }
				}
			}
			return default(T);
		}

		private void HandleError(object sender, ErrorEventArgs e)
		{
			e.ErrorContext.Handled = true;
		}
    }
}
