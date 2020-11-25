
<#
.SYNOPSIS
Update an APIM API with an openapi definition

.DESCRIPTION
Update an APIM API with a openapi definition

.PARAMETER ApimResourceGroup
The name of the resource group that contains the APIM instnace

.PARAMETER ApimServiceName
The name of the APIM instance

.PARAMETER ApiNames
The name of the API to update

.PARAMETER policyFilePath
The path to the policy file.

.EXAMPLE
Import-ApimOpenApiDefinitionFromFile -ApimResourceGroup dfc-foo-bar-rg -InstanceName dfc-foo-bar-apim -ApiName bar -OpenApiSpecificationFile some-file.yaml -Verbose

#>
[CmdletBinding()]
Param(
    [Parameter(Mandatory=$true)]
    [String]$ApimResourceGroup,
    [Parameter(Mandatory=$true)]
    [String]$ApimServiceName,
    [Parameter(Mandatory=$true)]
    [String]$ApiNames,
    [Parameter(Mandatory=$true)]
    [String]$policyFilePath
)

try {
    $context = New-AzApiManagementContext -ResourceGroupName $ApimResourceGroup -ServiceName $ApimServiceName

    $api = Get-AzApiManagementApi -Context $context -Name $ApiNames
    
    $apiOperations = Get-AzApiManagementOperation -Context $context -ApiId $api.ApiId
    
    $apiOperationNames = $apiOperations.Name
    
    foreach ($apiName in $ApiNames) {
        $api = ""
    
        $api = Get-AzApiManagementApi -Context $context -Name $apiName
    
        foreach ($apiOperationName in $apiOperationNames) {
            $operation = ""
    
            $operation = Get-AzApiManagementOperation -Context $context -ApiId $api.ApiId | Where-Object {$_.Name -eq $apiOperationName}
    
            Write-Output "Applying policy to API: $($api.name) Operation: $($operation.name)"
    
            Set-AzApiManagementPolicy -Context $context -Format application/vnd.ms-azure-apim.policy.raw+xml -ApiId $api.ApiId -OperationId $operation.OperationId -PolicyFilePath $PolicyFilePath -Verbose
        }
    }   
}
catch {
    throw $_    
}