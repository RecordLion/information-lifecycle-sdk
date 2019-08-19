# Information Lifecycle SDK Windows Toolkit
The Windows Toolkit allow you to build Windows-based applications that integrate
with Information Lifecycle Server remotely.  

## Assemblies
The assemblies included with the Information Lifecycle Windows Toolkit are listed in 
the table below.  
The following overviews provide additional information on the SDK assemblies:
* [Client overview](/docs/rmclient-overview.md)
* [Controls overview](/docs/rmclient-controls-overview.md) 

SDK Assembly | Description
---  | ---
RecordLion.RecordsManager.Client.dll | Contains a wrapper around the Information Lifecycle REST API to easily interact with Information Lifecycle Server.
RecordLion.RecordsManager.PhysicalAssets.Client | Contains a wrapper around the Physical Records Management REST API to easily interact with Information Lifecycle Server hosting PRM.
RecordLion.RecordsManager.Client.Controls.dll | Contain a set of reusable Windows UI Components that can be used to build Windows Applications.
RecordLion.Resources.dll | Contains localized resources

## Third-Party Dependencies
The third-party dependencies listed below are required for buildling custom integrations.  

Dependency | Description
---  | ---
Microsoft.IdentityModel.dll (v3.5) | Contains Window Identity Foundation Framework.  This can be installed from the Microsoft Downloads Site or if you have using Windows Server 2012, you should install using: `Add Windows Role and Features\Features\Windows Communication Foundation v3.5`
Newtonsoft.Json.dll	| This assembly contains the JSON.NET parser for parsing JSON.  It is included in the SDK, but can also be referenced from Nuget under the package name JSON.NET.
PresentationCore.dll | Core WPF Dependency – Required only if using RecordLion.RecordsManager.Client.Controls.dll
PresentationFramework.dll |	Core WPF Dependency – Required only if using RecordLion.RecordsManager.Client.Controls.dll
System.Windows.Controls.dll | Core WPF Dependency – Required only if using RecordLion.RecordsManager.Client.Controls.dll
WindowsBase.dll	| Core WPF Dependency – Required only if using RecordLion.RecordsManager.Client.Controls.dll

