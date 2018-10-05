#PowerShell

param(

 [Parameter(Mandatory=$True)]
 [string]
 $filePath,

 [Parameter(Mandatory=$True)]
 [string]
 $apiUrl,

 [Parameter(Mandatory=$True)]
 [string]
 $resourceGroup, 
 
 [Parameter(Mandatory=$True)]
 [string]
 $resourceName
)

Write-Host "Logging in...";
#Login-AzureRmAccount
$clientID = ""
$key = ""
$SecurePassword = $key | ConvertTo-SecureString -AsPlainText -Force
$cred = new-object -typename System.Management.Automation.PSCredential `
     -argumentlist $clientID, $SecurePassword
$tenantID = ""
Add-AzureRmAccount -Credential $cred -TenantId $tenantID -ServicePrincipal

$credentials = Invoke-AzureRmResourceAction -ResourceGroupName $resourceGroup -ResourceType Microsoft.Web/sites/config -ResourceName $resourceName/publishingcredentials -Action list -ApiVersion 2015-08-01 -Force
$username = $credentials.Properties.PublishingUserName
$password = $credentials.Properties.PublishingPassword


$base64AuthInfo = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes(("{0}:{1}" -f $username, $password)))
$userAgent = "powershell/1.0"
Invoke-RestMethod -Uri $apiUrl -Headers @{Authorization=("Basic {0}" -f $base64AuthInfo)} -UserAgent $userAgent -Method POST -InFile $filePath -ContentType "multipart/form-data"
