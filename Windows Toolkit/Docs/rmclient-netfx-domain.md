# Information Lifecycle Client Object Model
## Domain Objects Component
---
There are many domain objects in the client side object model (CSOM). These domain
objects, or entities, are used to marshall data from the Information Lifecycle API (API)
into objects a developer can easily use when writing source code.

>The following list includes some but not all of the domain objects available in the 
CSOM.  
>Explore the source code to find additional objects to meet your specific requirements. 

## Classes
---
`RetentionTrigger`  
blah

`EventOccurrence`  
blah

`IClientPagedItems`  
This interface represents a paged collection of results from a API call. Often, the
CSOM only exposes methods that return paged results in order to preserve system
resources on the server and network. The Integrations Sample App demonstrates how to
work with paged results in the `RetentionTriggerIntegrations` class.