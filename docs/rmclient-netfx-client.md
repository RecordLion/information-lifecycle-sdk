# Information Lifecycle Client Object Model
## Client Component

The `RecordsManagerClient` class implements the `IRecordsManagerClient` interface
and makes it easy for the developer to call the Information Lifecycle API (API). It is
the central object used to remotely communicate with the Server. Several constructors
exist for quickly creating instances.  

>The developer must ensure the `Url` and `Credentials` properties are set before
executing any API requests.

**Namespace:** RecordLion.RecordsManager.Client  
**Assembly:** RecordLion.RecordsManager.Client.dll

## Syntax
```cs
public sealed class RecordsManagerClient : IRecordsManagerClient
```

## Constructors
`RecordsManagerClient()`  
Creates an instance of the client using default Windows network credentials.

`RecordsManagerClient(string url)`  
Creates an instance of the client using the specified URL to the server and 
default Windows network credentials.

`RecordsManagerClient(string url, RecordsManagerCredentials credentials)`  
Creates an instance of the client using the specified URL to the server and the 
specified `RecordsManagerCredentials` credentials.

`RecordsManagerClient(string url, RecordsManagerCredentials credentials, ISecurityTokenRequestor securityTokenRequestor)`  
Creates an instance of the client using the specified URL to the server, the 
specified `RecordsManagerCredentials` credentials, and the specified `ISecurityTokenRequestor`
security token requestor.

`RecordsManagerClient(string url, CookieContainer cookieContainer)`  
Creates an instance of the client using the specified URL to the server and the specified
`CookieContainer` instance.

## Properties
`Url: string`  
Gets or sets the base URL for the API server.

`Credentials: RecordsManagerCredentials`  
Gets or sets the credentials used to authenticate with the API server.
See the [auth](/docs/rmclient-netfx-auth.md) section.

`Timeout: int`  
Gets or sets the timeout interval when executing API requests.

`CookieContainers: CookieContainer`  
Gets or sets the `CookieContainer` used when executing API requests.
    
`SecurityTokenRequestor: ISecurityTokenRequestor`  
Gets or sets the `ISecurityTokenRequestor` to authenticate Claims users.

`Issuer: string`  
Gets the Issuer response from the Information Lifecycle STS server's WS-Trust endpoint to 
authenticate Claims users.

## Examples
The following examples demonstrate several uses of the Client.
### Getting a Record by ID
```cs
//Create a new client using default network credentials
IRecordsManagerClient client = new RecordsManagerClient(url);
Record record = client.GetRecord("{03E593CB-AF1A-4D5C-9BCA-3ADB8DE24C8F}");
```

### Getting a Record by Uri
```cs
//Create a new client using default network credentials
IRecordsManagerClient client = new RecordsManagerClient(url);
Record record = client.GetRecordByUri("http://full_url_of_item");
```

### Searching For Records
It is possible to search with a partial Uri or Title as well.
```cs
//Create a new client using default network credentials
IRecordsManagerClient client = new RecordsManagerClient(url);
IEnumerable<Record> records = client.SearchRecords("TitleOrUri");
```

### Mark a Managed Item/Document as a Declared Record
An item/document can only be a declared record if it is managed by Records Manager.
Typically, this means a Connector has classified the item/document in Records Manager.
In Records Manager, this process is known as Recordizing an item/document. 
```cs
//Create a new client using default network credentials
IRecordsManagerClient client = new RecordsManagerClient(url);
client.DeclareRecord("http://full_url_of_item"); 
```

### Recordizing an Item/Document
This allows Information Lifecycle to track an item/document. Typically, a Connector will
perform Recordization during its classification job.
>Manual classification will force Information Lifecycle to use the assigned record class ID.  
>To use automatic classification, set `IsManuallyClassified` to `false` and do not send a record class ID.
```cs
//Create a new client using default network credentials
IRecordsManagerClient client = new RecordsManagerClient(url);
IEnumerable<Record> records = client.ProcessRecordization(
    new List<Recordize>()
    {
        new Recordize ()
        {
            State = RecordState.NewOrModified,
            Title = "Title",
            Uri = "http://full_url_of_item",
            Description = "Description of item",
            RecordClassId = 1,
            IsManuallyClassified = true //
        }
    });
```

### Removing an Item/Document (Un-Recordizing)
You can ask Information Lifecycle to remove an item/document and no longer track it. Typically,
a Connector will perform this action during its classification job if an item/document 
has been deleted.
```cs
//Create a new client using default network credentials
IRecordsManagerClient client = new RecordsManagerClient(url);
IEnumerable<Record> records = client.ProcessRecordization(
    new List<Recordize>()
    {
        new Recordize ()
        {
            State = RecordState .Removed,
            Uri = "http://full_url_of_file"
        }
    });
```

### Getting a Records Schedule
```cs
//Create a new client using default network credentials
IRecordsManagerClient client = new RecordsManagerClient(url);
Record record = client.GetRecordByUri("http://full_url_of_file");
Lifecycle schedule = client.GetLifecycle(record.LifecycleId);
```
