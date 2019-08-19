# $spHostUrl is the Url for the site to register.
# $siteUrl is the Url for the parent site collection.  
# If you are registering the root web of a site collection, $spHostUrl and $siteUrl will be the same
# $spocApi should reference your SP Online Connector Web, either on-premise or Gimmal SaaS

$spHostUrl = "http://customer.sharepoint.com/sites/site/web"
$siteUrl = "http://customer.sharepoint.com/sites/site"
$siteId = "07b000fd-7aa4-4e1a-b22f-3cb4564d90d0"
$webId = "07b000fd-7aa4-4e1a-b22f-3cb4564d90d9"
$runCrawl = "true"

$spocApi = "https://<SPOC-WEB>/API/Registration/RegisterSite?SPHostUrl={0}&SiteUrl={1}&SiteId={2}&WebId={3}&RunCrawl={4}" -f $spHostUrl, $siteUrl, $siteId, $webId, $runCrawl

$userName = "service-account@domain"
$password = "*******"
$base64AuthInfo = "Basic " + [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes(("{0}:{1}" -f $username,$password)))

$headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
$headers.Add("Content-Type", "application/json")
$result = Invoke-RestMethod -Uri $spocApi -Method POST -Headers $headers -Body ($base64AuthInfo | ConvertTo-Json)

ConvertTo-Json $result | Out-File "c:\temp\appRegistrations.json"
