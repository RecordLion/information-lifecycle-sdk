# $spHostUrl is the Url for the site (web) you wish to crawl.
# $spocApi should reference your SP Online Connector Web, either on-premise or Gimmal SaaS

$spHostUrl = "http://customer.sharepoint.com/sites/site/web"
$spocApi = "https://<SPOC-WEB>/API/OnDemand/CrawlSite?SPHostUrl={0}" -f $spHostUrl

$userName = "service-account@domain"
$password = "*******"
$base64AuthInfo = "Basic " + [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes(("{0}:{1}" -f $username,$password)))

$headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
$headers.Add("Content-Type", "application/json")
$result = Invoke-RestMethod -Uri $spocApi -Method POST -Headers $headers -Body ($base64AuthInfo | ConvertTo-Json)


ConvertTo-Json $result | Out-File "c:\temp\crawlSite.json"
