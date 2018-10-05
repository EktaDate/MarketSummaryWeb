<#
 .SYNOPSIS
    Deploys a template to Azure

 .DESCRIPTION
    Deploys an Azure Resource Manager template

 .PARAMETER subscriptionId
    The subscription id where the template will be deployed.

 .PARAMETER resourceGroupName
    The resource group where the template will be deployed. Can be the name of an existing or a new resource group.

 .PARAMETER resourceGroupLocation
    Optional, a resource group location. If specified, will try to create a new resource group in this location. If not specified, assumes resource group is existing.

 .PARAMETER deploymentName
    The deployment name.

 .PARAMETER templateFilePath
    Optional, path to the template file. Defaults to template.json.

 .PARAMETER parametersFilePath
    Optional, path to the parameters file. Defaults to parameters.json. If file is not found, will prompt for parameter values based on template.
#>

param(
 [Parameter(Mandatory=$True)]
 [string]
 $subscriptionId,

 [Parameter(Mandatory=$True)]
 [string]
 $resourceGroupName,
 
 [string]
 $prospectFunctionAppName ="mrsprospectsfunctionApp",
 
 [string]
 $prospectSummaryFunctionAppName ="mrsprospectsummaryfunctionapp",
 
 [string]
 $prospectFunctionAppAPIUrl = "https://mrsprospectsfunctionApp.scm.azurewebsites.net/api/zipdeploy",
 
 [string]
 $prospectSummaryFunctionAppAPIUrl ="https://mrsprospectsummaryfunctionapp.scm.azurewebsites.net/api/zipdeploy",
 
 [string]
 $prospectFunctionAppFilePath = "",
 
 [string]
 $prospectSummaryFunctionAppFilePath ="",
 
 [string]
 $clientID ="",
 
 [string]
 $key ="",
 
 [string]
 $tenantID ="",
 
 [string]
 $resourceGroupLocation,

 [string]
 $templateFilePath = "template.json",

 [string]
 $parametersFilePath = "parameters.json"
)

<#
.SYNOPSIS
    Registers RPs
#>
Function RegisterRP {
    Param(
        [string]$ResourceProviderNamespace
    )

    Write-Host "Registering resource provider '$ResourceProviderNamespace'";
    Register-AzureRmResourceProvider -ProviderNamespace $ResourceProviderNamespace;
}

#******************************************************************************
# Script body
# Execution begins here
#******************************************************************************
#$ErrorActionPreference = "Stop"

# sign in
Write-Host "Logging in...";
#Login-AzureRmAccount;

$SecurePassword = $key | ConvertTo-SecureString -AsPlainText -Force
$cred = new-object -typename System.Management.Automation.PSCredential `
     -argumentlist $clientID, $SecurePassword
Add-AzureRmAccount -Credential $cred -TenantId $tenantID -ServicePrincipal

# select subscription
Write-Host "Selecting subscription '$subscriptionId'";
Select-AzureRmSubscription -SubscriptionID $subscriptionId;

# Register RPs
$resourceProviders = @("microsoft.cognitiveservices","microsoft.logic","microsoft.sql","microsoft.storage","microsoft.web");
if($resourceProviders.length) {
    Write-Host "Registering resource providers"
    foreach($resourceProvider in $resourceProviders) {
        RegisterRP($resourceProvider);
    }
}

#Create or check for existing resource group
$resourceGroup = Get-AzureRmResourceGroup -Name $resourceGroupName -ErrorAction SilentlyContinue
if(!$resourceGroup)
{
    Write-Host "Resource group '$resourceGroupName' does not exist. Creating Resource Group in central India";
    if(!$resourceGroupLocation) {
        $resourceGroupLocation = "centralindia"
    }
    Write-Host "Creating resource group '$resourceGroupName' in location '$resourceGroupLocation'";
    New-AzureRmResourceGroup -Name $resourceGroupName -Location $resourceGroupLocation
}
else{
    Write-Host "Using existing resource group '$resourceGroupName'";
}

# Start the deployment
Write-Host "Starting deployment...";
if(Test-Path $parametersFilePath) {
    New-AzureRmResourceGroupDeployment -ResourceGroupName $resourceGroupName -TemplateFile $templateFilePath -TemplateParameterFile $parametersFilePath;
} else {
    New-AzureRmResourceGroupDeployment -ResourceGroupName $resourceGroupName -TemplateFile $templateFilePath;
}

Start-Sleep -s 120;
powershell ipconfig /flushdns;

Write-Host "ProspectFunction deployment...";

# powershell -f function.ps1 -filePath $prospectFunctionAppFilePath -apiUrl $prospectFunctionAppAPIUrl -resourceGroup $resourceGroupName -resourceName $prospectFunctionAppName;

$credentials = Invoke-AzureRmResourceAction -ResourceGroupName $resourceGroupName -ResourceType Microsoft.Web/sites/config -ResourceName $prospectFunctionAppName/publishingcredentials -Action list -ApiVersion 2015-08-01 -Force
$username = $credentials.Properties.PublishingUserName
$password = $credentials.Properties.PublishingPassword


$base64AuthInfo = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes(("{0}:{1}" -f $username, $password)))
$userAgent = "powershell/1.0"
Invoke-RestMethod -Uri $prospectFunctionAppAPIUrl -Headers @{Authorization=("Basic {0}" -f $base64AuthInfo)} -UserAgent $userAgent -Method POST -InFile $prospectFunctionAppFilePath -ContentType "multipart/form-data"


powershell ipconfig /flushdns;
Start-Sleep -s 120;

Write-Host "Logic App Deployment...";

$templateFilePath = "logicApp_template.json";
$parametersFilePath = "logicApp_parameters.json";
New-AzureRmResourceGroupDeployment -ResourceGroupName $resourceGroupName -TemplateFile $templateFilePath -TemplateParameterFile $parametersFilePath;
powershell ipconfig /flushdns;

Write-Host "ProspectSummary deployment...";
# powershell -f function.ps1 -filePath $prospectSummaryFunctionAppFilePath -apiUrl $prospectSummaryFunctionAppAPIUrl -resourceGroup $resourceGroupName -resourceName $prospectSummaryFunctionAppName;

$credentials = Invoke-AzureRmResourceAction -ResourceGroupName $resourceGroupName -ResourceType Microsoft.Web/sites/config -ResourceName $prospectSummaryFunctionAppName/publishingcredentials -Action list -ApiVersion 2015-08-01 -Force
$username = $credentials.Properties.PublishingUserName
$password = $credentials.Properties.PublishingPassword

$base64AuthInfo = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes(("{0}:{1}" -f $username, $password)))
$userAgent = "powershell/1.0"
Invoke-RestMethod -Uri $prospectSummaryFunctionAppAPIUrl -Headers @{Authorization=("Basic {0}" -f $base64AuthInfo)} -UserAgent $userAgent -Method POST -InFile $prospectSummaryFunctionAppFilePath -ContentType "multipart/form-data"


Write-Host "Web App deployment";
$templateFilePath = "webApp_template.json";
$parametersFilePath = "weApp_parameters.json";
New-AzureRmResourceGroupDeployment -ResourceGroupName $resourceGroupName -TemplateFile $templateFilePath -TemplateParameterFile $parametersFilePath;
powershell ipconfig /flushdns;

$PropertiesObject = @{
    "enabled" = "True";
    "unauthenticatedClientAction" = "0";
    "defaultProvider" = "0";
    "tokenStoreEnabled" = "True";
    "clientId" = "";
    "issuer" = "https://sts.windows.net//";
    "allowedAudiences" = "https://mrsmarketSummary.azurewebsites.net/.auth/login/aad/callback";
    "isAadAutoProvisioned" = "True";
    "aadClientId" = "";
    "openIdIssuer" = "https://sts.windows.net//";
}

$resourceType = "Microsoft.Web/sites/config"
$resourceName = "mrsmarketSummary/authsettings"

New-AzureRmResource -PropertyObject $PropertiesObject `
-ResourceGroupName $resourceGroupName -ResourceType $resourceType `
-ResourceName $resourcename -ApiVersion 2015-08-01 -Force

Write-Host "Deployment completed successfully.";
