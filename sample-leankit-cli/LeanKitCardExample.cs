using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace leankit.cli
{
    public class LeanKitCardExample
	{
	    private static HttpClient _client = new HttpClient();
	    public LeanKitCardExample(string hostName, string userName, string password)
	    {
	        _client.BaseAddress = new Uri(string.Format("https://{0}.leankit.com", hostName));
	        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

	        // Basic Authentication
	        var authByteArray = Encoding.ASCII.GetBytes(string.Format("{0}:{1}", userName, password));
	        var authString = Convert.ToBase64String(authByteArray);
	        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authString);

	        // Alternatively, you can use Token authentication instead of Basic Authentication
	        // _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

	    }

	    public async Task<CardResponse> GetCard(long cardId)
	    {
	        var path = new Uri(string.Format("/io/card/{0}", cardId), UriKind.RelativeOrAbsolute);
	        var res = await _client.GetStringAsync(path);
	        return JsonConvert.DeserializeObject<CardResponse>(res);
	    }

	    public async Task<CardResponse> UpdateCustomField(long cardId, long fieldId, int fieldIndex, string customFieldValue)
	    {
	        var path = new Uri(string.Format("/io/card/{0}", cardId), UriKind.RelativeOrAbsolute);
	        var ops = new[]
	        {
	                new {
	                    op = "replace",
	                    path = string.Format("/customFields/{0}", fieldIndex),
	                    value = new
	                    {
	                        fieldId = fieldId.ToString(),
	                        value = customFieldValue
	                    }
	                }
	            };
	        var body = JsonConvert.SerializeObject(ops);
	        var request = new HttpRequestMessage(new HttpMethod("PATCH"), path)
	        {
	            Content = new StringContent(body, Encoding.UTF8, "application/json")
	        };
	        var response = await _client.SendAsync(request);
	        response.EnsureSuccessStatusCode();
	        if (response.Content != null)
	        {
	            var str = await response.Content.ReadAsStringAsync();
	            return JsonConvert.DeserializeObject<CardResponse>(str);
	        }
	        return null;
	    }
	}

	public class BlockedStatus
	{
	    public bool IsBlocked { get; set; }
	    public string Reason { get; set; }
	    public DateTime? Date { get; set; }
	}

	public class CardBoardResponse
	{
	    public long Id { get; set; }
	    public string Title { get; set; }
	    public long Version { get; set; }
	    public bool IsArchived { get; set; }
	}

	public class CardLaneResponse
	{
	    public long Id { get; set; }
	    public int CardLimit { get; set; }
	    public string Description { get; set; }
	    public int Index { get; set; }
	    public string LaneClassType { get; set; }
	    public string LaneType { get; set; }
	    public string Orientation { get; set; }
	    public string Title { get; set; }
	    // TODO: taskBoard
	}

	public class CardTypeResponse
	{
	    public long Id { get; set; }
	    public string Title { get; set; }
	    public string CardColor { get; set; }
	    public string IconColor { get; set; }
	    public string IconName { get; set; }
	    public string IconPath { get; set; }
	}

	public class CardUserResponse
	{
	    public long Id { get; set; }
	    public string EmailAddress { get; set; }
	    public string FullName { get; set; }
	    public string Avatar { get; set; }
	}

	public class CardCustomFieldResponse
	{
	    public long FieldId { get; set; }
	    public string Type { get; set; }
	    public string Label { get; set; }
	    public string Value { get; set; }
	}

	public class CardResponse
	{
	    public CardResponse()
	    {
	        Tags = new List<string>();
	        CustomFields = new List<CardCustomFieldResponse>();
	    }
	    public long Id { get; set; }
	    public CardBoardResponse Board { get; set; }
	    public CardLaneResponse Lane { get; set; }
	    public CardTypeResponse Type { get; set; }
	    public long Index { get; set; }
	    public long Version { get; set; }
	    public string Title { get; set; }
	    public string Description { get; set; }
	    public string Priority { get; set; }
	    public int Size { get; set; }
	    public DateTime? PlannedStart { get; set; }
	    public DateTime? PlannedFinish { get; set; }
	    public DateTime? ActualStart { get; set; }
	    public DateTime? ActualFinish { get; set; }
	    public DateTime CreatedOn { get; set; }
	    public DateTime UpdatedOn { get; set; }
	    public DateTime? MovedOn { get; set; }
	    public DateTime? ArchivedOn { get; set; }
	    public List<string> Tags { get; set; }
	    public BlockedStatus BlockedStatus { get; set; }
	    public string Color { get; set; }
	    public CardUserResponse CreatedBy { get; set; }
	    public CardUserResponse UpdatedBy { get; set; }
	    public CardUserResponse MovedBy { get; set; }
	    public CardUserResponse ArchivedBy { get; set; }
	    public List<CardCustomFieldResponse> CustomFields { get; set; }
	}

}
