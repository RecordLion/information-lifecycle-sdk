# $spocApi should reference your SP Online Connector Web, either on-premise or Gimmal SaaS

$spocApi = "https://<SPOC-WEB>/API/Registration/ActivateAll
$userName = "service-account@domain"
$password = "*******"
$base64AuthInfo = "Basic " + [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes(("{0}:{1}" -f $username,$password)))

$headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
$headers.Add("Content-Type", "application/json")
$result = Invoke-RestMethod -Uri $spocApi -Method POST -Headers $headers -Body ($base64AuthInfo | ConvertTo-Json)


ConvertTo-Json $result | Out-File "c:\temp\appActivate.json"

