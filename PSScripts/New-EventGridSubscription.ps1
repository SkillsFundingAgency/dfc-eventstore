<#
.SYNOPSIS
Gets the available AKS upgrades and writes the information out to logs and a variable.  

.DESCRIPTION
Gets the available AKS upgrades and writes the information out to logs and a variable.  The az cli doesn't handle writing complex objects to Azure DevOps variables well so a simple count is outputted along with more detail to the logs.

.PARAMETER AksResourceGroup
The AKS resource group

.PARAMETER AksServiceName
The AKS service name
#>

[CmdletBinding()]
param(
    [Parameter(Mandatory=$true)]
    [String]$TopicResourceGroup,
    [Parameter(Mandatory=$true)]
    [String]$SubscriptionId
)


az extension add --name eventgrid

az eventgrid event-subscription create `
--name dfc-eventstore `
--source-resource-id /subscriptions/962cae10-2950-412a-93e3-d8ae92b17896/resourceGroups/dfc-dev-stax-editor-rg/providers/Microsoft.EventGrid/topics/dfc-dev-stax-egt `
--endpoint /subscriptions/962cae10-2950-412a-93e3-d8ae92b17896/resourceGroups/dfc-dev-app-eventstore-rg/providers/Microsoft.Web/sites/dfc-dev-app-eventstore-fa/functions/Execute `
--endpoint-type azurefunction

