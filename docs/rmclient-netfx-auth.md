# Information Lifecycle Client Object Model
## Authentication Component

The `RecordsManagerCredentials` class lets a developer connect to the 
Information Lifecycle API (API) using one of the following `RecordsManagerCredentialType`
enumerations:
* Forms
* Claims

### Forms
---
We strongly recommended using `Forms` for your custom SDK integrations.  
This is used when connecting as a Service to the API.  
Information Lifecycle Connector services communicate with the API using `Forms`.  
>Service accounts are created in Information Lifecycle and are local accounts **not** domain accounts.

#### Code Example - Service Account

```cs
var forms = new RecordsManagerCredentials("sp-service", "password", RecordsManagerCredentialType.Forms);
```

### Claims
---
The `RecordsManagerCredentialType.Claims` enum is used when connecting as a specific user
to the API. Typically, this is a Windows domain account. This approach introduces additional
complexity. It is not recommended for custom SDK integrations, unless it is required to
restrict a user to only the resources they can access.

#### Code Example - Default Network Credentials

```cs
var claims = new RecordsManagerCredentials();
```

#### Code Example - Specific User Credentials

```cs
var claims = new RecordsManagerCredentials("domain\\user", "password", RecordsManagerCredentialType.Claims);
```

### Information Lifecycle Service (SaaS)
---

If you're connecting to the RecordLion Information Lifecycle Service in the cloud, you should reference the `RecordLion.RecordsManager.Client.Azure` project which will require .NET 4.5+

This project provides the `RecordLion.RecordsManager.Client.RecordsManagerClientFactory` class that will properly construct an instance of `RecordsManagerClient` that can properly authenticate with Information Lifecycle on-premise or the Information Lifecycle Service in the cloud.

All constructoer overloads of `RecordsManagerClient` are represented by `RecordsManagerClientFactory.Create`

#### Code Example - Connecting to the cloud-based Information Lifecycle Service (or Information Lifecycle on-premise)

```cs
var client = RecordsManagerClientFactory.Create("domain\\user", "password");
```