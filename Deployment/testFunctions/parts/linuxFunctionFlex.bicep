param prefix string
param title string
param registrationName string
param functionName string
param location string
param testPath string
param sku string = ''
param sortOrder int
param useDotnetIsolated bool = false
var deploymentStorageContainerName = 'deployments'

@allowed(['dotnet', 'dotnet-isolated', 'node', 'java', 'powershell', 'python'])
param workerRuntime string

@allowed(['Csharp', 'Nodejs', 'Python', 'Java', 'Fsharp'])
param language string

@allowed(['8.0', '20'])
param functionAppRuntimeVersion string

resource storageAccount 'Microsoft.Storage/storageAccounts@2021-04-01' = {
  name: '${functionName}stor'
  location: location
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
  }
  properties: {
    minimumTlsVersion: 'TLS1_2'
    allowBlobPublicAccess: false
    supportsHttpsTrafficOnly: true
    accessTier: 'Hot'
    networkAcls: {
      defaultAction: 'Allow'
    }
  }

  resource fileServices 'fileServices' = {
    name: 'default'
  }

  resource tableServices 'tableServices' = {
    name: 'default'
  }

  resource blobServices 'blobServices' = {
    name: 'default'

    resource deployments 'containers' = {
      name: deploymentStorageContainerName
    }
  }
}

resource functionFarm 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: '${functionName}plan'
  location: location
  kind: 'linux'
  sku: {
    tier: 'FlexConsumption'
    name: 'FC1'
  }
  properties: {
    reserved: true
  }
}

resource function 'Microsoft.Web/sites@2024-04-01' = {
  name: functionName
  location: location
  kind: 'functionapp,linux'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: functionFarm.id
    httpsOnly: true
    functionAppConfig: {
      deployment: {
        storage: {
          type: 'blobContainer'
          value: '${storageAccount.properties.primaryEndpoints.blob}${deploymentStorageContainerName}'
          authentication: {
            type: 'SystemAssignedIdentity'
          }
        }
      }
      scaleAndConcurrency: {
        // 40 is the minimum
        maximumInstanceCount: 40
        instanceMemoryMB: 2048
      }
      runtime: { 
        name: workerRuntime
        version: functionAppRuntimeVersion
      }
    }
    siteConfig: {
      ftpsState: 'FtpsOnly'
      minTlsVersion: '1.2'
      scmMinTlsVersion: '1.2'
      http20Enabled: true
      appSettings: [
        {
          name: 'AzureWebJobsStorage__accountName'
          value: storageAccount.name
        }
      ]
    }
  }
}

var defaultHostKey = listkeys('${function.id}/host/default', '2016-08-01').functionKeys.default
module configurationRegistration 'configurationRegistration.bicep' = {
  name: '${deployment().name}-configurationRegistration'
  params: {
    title: title
    registrationName: registrationName
    prefix: prefix
    language: language
    runtime: 'FunctionsV4'
    hostEnvironment: 'Linux'
    authenticationHeaderName: 'x-functions-key'
    authenticationHeaderValue: defaultHostKey
    testEndpoint: 'https://${function.properties.defaultHostName}${testPath}'
    sku: sku
    sortOrder: sortOrder
  }
}

var storageRoleDefinitionId  = 'b7e6dc6d-f1e8-4753-8033-0f276bb0955b' //Storage Blob Data Owner role

// Allow access from function app to storage account using a managed identity
resource storageRoleAssignment 'Microsoft.Authorization/roleAssignments@2020-04-01-preview' = {
  name: guid(storageAccount.id, storageRoleDefinitionId)
  scope: storageAccount
  properties: {
    roleDefinitionId: resourceId('Microsoft.Authorization/roleDefinitions', storageRoleDefinitionId)
    principalId: function.identity.principalId
    principalType: 'ServicePrincipal'
  }
}
