<#
.SYNOPSIS
Creates EventGrid Subscription.  

.DESCRIPTION
Creates EventGrid Subscription.  The topic and function app must already exist along with their resourcegroups

.PARAMETER EventGridSubscriptionName
The EventGrid Subscription Name

.PARAMETER SubscriptionId
The Azure Subscription Id

.PARAMETER TopicResourceGroup
The Topic ResourceGroup

.PARAMETER Topic
The name of the Topic

.PARAMETER FunctionAppResourceGroup
The Function App ResourceGroup

.PARAMETER FunctionApp
The name of the Function App
#>

[CmdletBinding()]
param(
    [Parameter(Mandatory=$true)]
    [String]$EventGridSubscriptionName,
    [Parameter(Mandatory=$true)]
    [String]$SubscriptionId,
    [Parameter(Mandatory=$true)]
    [String]$TopicResourceGroup,
    [Parameter(Mandatory=$true)]
    [String]$Topic,
    [Parameter(Mandatory=$true)]
    [String]$FunctionAppResourceGroup,
    [Parameter(Mandatory=$true)]
    [String]$FunctionApp
)

# this is to ensure that the latest extension of AZ CLI eventgrid has been installed
az extension add --name eventgrid

# Create the event grid subscriptionfrom an existing custom topic
az eventgrid event-subscription create `
--name $EventGridSubscriptionName `
--source-resource-id "/subscriptions/$($SubscriptionId)/resourceGroups/$($TopicResourceGroup)/providers/Microsoft.EventGrid/topics/$($Topic)" `
--endpoint "/subscriptions/$($SubscriptionId)/resourceGroups/$($FunctionAppResourceGroup)/providers/Microsoft.Web/sites/$($FunctionApp)/functions/Execute" `
--endpoint-type azurefunction

