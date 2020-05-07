# LeanKit.API.Client.Library

### DEPRECATION NOTICE
**This library is deprecated and has been unsupported since January 25, 2019. Additionally, it uses the LeanKit v1 API which is also deprecated. Please [use our v2 API directly](https://success.planview.com/Planview_LeanKit/LeanKit_API/01_v2) instead of this library.**

The **LeanKit.API.Client.Library** namespace provides a wrapper library designed to simplify the integration of external systems and utilities with your LeanKit account. This library exposes mechanisms that support both interactions with LeanKit as well as a strongly-typed object model representing the main entities within LeanKit.

This library exposes two primary ways to interact with LeanKit. These are the LeanKitClient and LeanKitIntegration classes. Both of these are explained below.

## Installing with NuGet

NuGet is a Visual Studio extension that makes it easy to add, remove, and update libraries and tools in Visual Studio projects that use the .NET Framework. To learn more about NuGet, read the [documentation](http://docs.nuget.org).

To install the [LeanKit API Client](https://www.nuget.org/packages/LeanKit.API.Client) using NuGet, search for "LeanKit" in the Visual Studio NuGet Package Manager, or install directly using the Package Manager Console:

```
PM> Install-Package LeanKit.API.Client
```

## LeanKitClient

This class exposes methods that interact directly with LeanKit API. This class exposes the same methods exposed through the REST API, but hides the complexity of working with HTTP and JSON. This class allows you to send commands and queries to LeanKit using strongly-typed native objects. This class should be used if you are building a simple, stateless integration where you are not interested in interacting with a LeanKit Board over a period of time. This class should also be used if you cannot support a long-running service or process.  

### Connecting using LeanKitClient

```csharp
var leanKitAuth = new LeanKitBasicAuth
	{
		Hostname = "MyAccount", 
			   // Only the account name portion of the URL
			   // e.g. https://MyAccount.leankit.com
		Username = "your-account-email-address",
		Password = "your-account-password"
	};
var api = new LeanKitClientFactory().Create(leanKitAuth);
```

#### Connecting to alternate LeanKit domains

To connect to other LeanKit domains, such as LeanKit for Construction [leankit.co](http://leankit.co), use the `UrlTemplateOverride` property.

```csharp
var leanKitAuth = new LeanKitBasicAuth
	{
		Hostname = "MyAccount", 
			   // Only the account name portion of the URL
			   // e.g. https://MyAccount.leankit.com
		Username = "your-account-email-address",
		Password = "your-account-password",
		UrlTemplateOverride = "https://{0}.leankit.co"
	};
var api = new LeanKitClientFactory().Create(leanKitAuth);
```

### Sample Helper Class in C&#35;

```csharp
using System.Collections.Generic;
using System.Linq;
using LeanKit.API.Client.Library;
using LeanKit.API.Client.Library.TransferObjects;

namespace CardUpdateTest
{
	public class LeanKitHelper
	{
		readonly ILeanKitApi _api;
		public LeanKitHelper(string hostName, string emailAddress, string password)
		{
			_api = CreateApiClient(hostName, emailAddress, password);
		}

		private ILeanKitApi CreateApiClient(string hostName, string emailAddress, string password)
		{
			var auth = new LeanKitBasicAuth
			{
				Hostname = hostName,
				Username = emailAddress,
				Password = password
			};

			var api = new LeanKitClientFactory().Create(auth);
			return api;
		}

		public List<BoardListing> GetBoards()
		{
			var boards = _api.GetBoards().ToList();
			return boards;
		}

		public Board GetBoard(long boardId)
		{
			return _api.GetBoard(boardId);
		}

	}
}
```

### Sample Helper Class in Visual Basic .NET

```vbnet
Imports LeanKit.API.Client.Library
Imports LeanKit.API.Client.Library.TransferObjects

Public Class LeanKitHelper

    Dim ReadOnly _api As ILeanKitApi

    Public Sub New(hostName As String, emailAddress As String, password As String)
        _api = CreateApiClient(hostName, emailAddress, password)
    End Sub

    Private Function CreateApiClient(hostName As String, emailAddress As String, password As String) As ILeanKitApi
        Dim auth as LeanKitBasicAuth
        auth = New LeanKitBasicAuth()
        auth.Hostname = hostName
        auth.Username = emailAddress
        auth.Password = password
        
        Dim api as ILeanKitApi
        api = new LeanKitClientFactory().Create(auth)
        CreateApiClient = api
    End Function

    Public Function GetBoards() As List(Of BoardListing)
        Dim boards As List(Of BoardListing)
        boards = _api.GetBoards()
        GetBoards = boards
    End Function

    Public Function GetBoard(boardId As Long) As Board
        GetBoard = _api.GetBoard(boardId)
    End Function
        
End Class
```

## LeanKitIntegration

This class is designed to be used in stateful implementations. This class monitors the changes to a Board, and raises events whenever a board is changed. This library helps reduce the complexity of polling for changes. In addition, this class exposes the same command and query methods that are available in the LeanKitClient class. Leveraging its stateful nature, many of these queries and commands can be optimized, and provide more powerful validation.

This class is designed to retrieve and hold a reference to the board, and therefore should not be instantiated numerous times. Each instance will be associated to a single LeanKit Board. You will need to create an instance for each Board you wish to interact with.

### Connecting using LeanKitIntegration

```csharp
var leanKitAuth = new LeanKitBasicAuth
	{
		Hostname = "MyAccount", 
			   // Only the account name portion of the URL
			   // e.g. https://MyAccount.leankit.com
		Username = "your-account-email-address",
		Password = "your-account-password"
	};

// Use the board ID (number) of the board you wish to monitor
// This ID can be found in the URL when you visit your LeanKit board
// https://accountname.leankit.com/Boards/View/101
var boardId = 101;

var integration = new LeanKitIntegrationFactory().Create(boardId, leanKitAuth);
integration.BoardChanged += integration_BoardChanged;
integration.StartWatching();
```

## Connecting through an HTTP proxy server

To connect to the LeanKit API through an HTTP proxy server, you will need to add or update your .NET application's `app.config` or `web.config` file to include a `<defaultProxy>` section.

```xml
<system.net>
  <defaultProxy useDefaultCredentials="true">
    <proxy bypassonlocal="true" usesystemdefault="true" />
  </defaultProxy>
</system.net>
```

Or, if more granular control of the proxy is required, it could look something like:

```xml
<system.net>
  <defaultProxy enabled="true" useDefaultCredentials="false">
    <proxy usesystemdefault="true" proxyaddress="http://192.168.1.10:3128" bypassonlocal="true" />
  </defaultProxy>
</system.net>
```

For more help, please review the .NET documentation on [proxy configuration](https://msdn.microsoft.com/en-us/library/kd3cf2ex(v=vs.110).aspx).

### LeanKitClient Interface

```csharp
IEnumerable<BoardListing> GetBoards();
Board GetBoard(long boardId);
IEnumerable<Lane> GetBacklogLanes(long boardId);
IEnumerable<HierarchicalLane> GetArchiveLanes(long boardId);
IEnumerable<CardView> GetArchiveCards(long boardId);
Board GetNewerIfExists(long boardId, int version);
IEnumerable<BoardHistoryEvent> GetBoardHistorySince(long boardId, int version);
Card GetCard(long boardId, long cardId);
Card GetCardByExternalId(long boardId, string externalCardId);
BoardIdentifiers GetBoardIdentifiers(long boardId);
long MoveCard(long boardId, long cardId, long toLaneId, int position ,string wipOverrideReason);
long MoveCard(long boardId, long cardId, long toLaneId, int position);
CardAddResult AddCard(long boardId, Card newCard, string wipOverrideReason);
CardAddResult AddCard(long boardId, Card newCard);
IEnumerable<Card> AddCards(long boardId, IEnumerable<Card> newCards, string wipOverrideReason);
IEnumerable<Card> AddCards(long boardId, IEnumerable<Card> newCards);
CardUpdateResult UpdateCard(long boardId, Card updatedCard, string wipOverrideReason);
CardUpdateResult UpdateCard(long boardId, Card updatedCard);
CardsUpdateResult UpdateCards(long boardId, IEnumerable<Card> updatedCards, string wipOverrideReason);
CardsUpdateResult UpdateCards(long boardId, IEnumerable<Card> updatedCards);
long DeleteCard(long boardId, long cardId);
CardsDeleteResult DeleteCards(long boardId, IEnumerable<long> cardIds);
BoardUpdates CheckForUpdates(long boardId, int version);
IEnumerable<Comment> GetComments(long boardId, long cardId);
int PostComment(long boardId, long cardId, Comment comment);
IEnumerable<CardEvent> GetCardHistory(long boardId, long cardId);
IEnumerable<CardView> SearchCards(long boardId, SearchOptions options);
IEnumerable<Asset> GetAttachments(long boardId, long cardId);
Asset GetAttachment(long boardId, long cardId, long attachmentId);
long SaveAttachment(long boardId, long cardId, string fileName, string description, string mimeType, byte[] fileBytes);
long DeleteAttachment(long boardId, long cardId, long attachmentId);
```

### LeanKitIntegration Interface

```csharp
event EventHandler<BoardStatusCheckedEventArgs> BoardStatusChecked;
event EventHandler<BoardInfoRefreshedEventArgs> BoardInfoRefreshed;
event EventHandler<BoardChangedEventArgs> BoardChanged;
void StartWatching();
void OnBoardStatusChecked(BoardStatusCheckedEventArgs eventArgs);
void OnBoardRefresh(BoardInfoRefreshedEventArgs eventArgs);
void OnBoardChanged(BoardChangedEventArgs eventArgs);
Card GetCard(long cardId);
Card GetCardByExternalId(long boardId, string externalCardId);
void AddCard(Card card);
void AddCard(Card card, string wipOverrideReason);
void UpdateCard(Card card);
void UpdateCard(Card card, string wipOverrideReason);
IEnumerable<Comment> GetComments(long cardId);
void PostComment(long cardId, Comment comment);
IEnumerable<CardEvent> GetCardHistory(long cardId);
void DeleteCard(long cardId);
void DeleteCards(IEnumerable<long> cardIds);
IEnumerable<Lane> GetArchive();
Board GetBoard();
void MoveCard(long cardId, long toLaneId, int position, string wipOverrideReason);
void MoveCard(long cardId, long toLaneId, int position);
IEnumerable<CardView> SearchCards(SearchOptions options);
IEnumerable<Asset> GetAttachments(long cardId);
Asset GetAttachment(long cardId, long attachmentId);
void SaveAttachment(long cardId, string fileName, string description, string mimeType, byte[] fileBytes);
void DeleteAttachment(long cardId, long attachmentId);

```

### BoardChangedEventArgs Properties

```csharp
public List<CardMoveEvent> MovedCards { get; set; }
public List<CardUpdateEvent> UpdatedCards { get; set; }
public List<CardAddEvent> AddedCards { get; set; }
public bool BoardStructureChanged { get; set; }
public Board UpdatedBoard { get; set; }
public List<CardDeletedEvent> DeletedCards { get; set; }
public List<CardBlockedEvent> BlockedCards { get; set; }
public List<CardUnBlockedEvent> UnBlockedCards { get; set; }
public List<CardUserAssignmentEvent> AssignedUsers { get; set; }
public List<CardUserUnAssignmentEvent> UnAssignedUsers { get; set; }
public List<CommentPostedEvent> PostedComments { get; set; }
public List<WipOverrideEvent> WipOverrides { get; set; }
public List<UserWipOverrideEvent> UserWipOverrides { get; set; }
public bool BoardWasReloaded { get; set; }
```

## Samples

Included are samples to help you understand how to use the stateless **LeanKitClient** or stateful **LeanKitIntegration** API libraries.

## Questions?

Visit [support.leankit.com](http://support.leankit.com).

## Copyright

Copyright &copy; 2013-2015 LeanKit Inc.

## License

LeanKit Integration Service is licensed under [MIT](http://www.opensource.org/licenses/mit-license.php). Refer to license.txt for more information.
