param functionStorageAccountName string
param sharedStorageName string
param configServiceName string
param resultsTableName string
param responsesContainerName string
param functionName string
param appInsightsName string
param location string

resource functionStorageAccount 'Microsoft.Storage/storageAccounts@2021-04-01' existing = {
  name: functionStorageAccountName
}

resource sharedStorageAccount 'Microsoft.Storage/storageAccounts@2021-04-01' existing = {
  name: sharedStorageName
}

resource appConfigurationService 'Microsoft.AppConfiguration/configurationStores@2022-05-01' existing = {
  name: configServiceName
}

resource StorageTableDataReaderRole 'Microsoft.Authorization/roleDefinitions@2022-04-01' existing = {
  name: '76199698-9eea-4c19-bc75-cec21354c6b6'
}

resource StorageBlobDataContributorRole 'Microsoft.Authorization/roleDefinitions@2022-04-01' existing = {
  name: 'ba92f5b4-2d11-453d-a403-e96b0029c9fe'
}

resource configServiceDataReaderRole 'Microsoft.Authorization/roleDefinitions@2022-04-01' existing = {
  name: '516239f1-63e1-4d78-a4de-a74fb236a071'
}

resource storageRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(sharedStorageAccount.id, StorageTableDataReaderRole.id)
  scope: sharedStorageAccount
  properties: {
    roleDefinitionId: StorageTableDataReaderRole.id
    principalId: function.identity.principalId
    principalType: 'ServicePrincipal'
  }
}

resource storageBlobContributorRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(sharedStorageAccount.id, StorageBlobDataContributorRole.id, function.id)
  scope: sharedStorageAccount
  properties: {
    roleDefinitionId: StorageBlobDataContributorRole.id
    principalId: function.identity.principalId
    principalType: 'ServicePrincipal'
  }
}

resource configServiceRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(functionStorageAccount.id, configServiceDataReaderRole.id)
  scope: appConfigurationService
  properties: {
    roleDefinitionId: configServiceDataReaderRole.id
    principalId: function.identity.principalId
    principalType: 'ServicePrincipal'
  }
}

resource functionFarm 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: '${functionName}plan'
  location: location
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
    size: 'Y1'
    family: 'Y'
    capacity: 0
  }
}

resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: appInsightsName
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
  }
}


resource function 'Microsoft.Web/sites@2022-03-01' = {
  name: functionName
  location: location
  kind: 'functionapp'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: functionFarm.id
    httpsOnly: true
    siteConfig: {
      ftpsState: 'FtpsOnly'
      minTlsVersion: '1.2'
      scmMinTlsVersion: '1.2'
      http20Enabled: true
      windowsFxVersion: 'dotnet-isolated|8.0'
      use32BitWorkerProcess: false
      appSettings: [
        {
          name: 'AzureWebJobsStorage'
          value: 'DefaultEndpointsProtocol=https;AccountName=${functionStorageAccount.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${functionStorageAccount.listKeys().keys[0].value}'
        }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: 'dotnet-isolated'
        }
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: appInsights.properties.InstrumentationKey
        }
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~4'
        }
        {
          name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
          value: 'DefaultEndpointsProtocol=https;AccountName=${functionStorageAccount.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${functionStorageAccount.listKeys().keys[0].value}'
        }
        {
          name: 'WEBSITE_RUN_FROM_PACKAGE'
          value: '1'
        }
        {
          name: 'WEBSITE_ENABLE_SYNC_UPDATE_SITE'
          value: 'true'
        }
        {
          name: 'WEBSITE_USE_PLACEHOLDER_DOTNETISOLATED'
          value: '1'
        }
        {
          name: 'dayRange'
          value: '7'
        }
        {
          name: 'storage__tableEndpoint'
          value: sharedStorageAccount.properties.primaryEndpoints.table
        }
        {
          name: 'storage__resultsTableName'
          value: resultsTableName
        }
        {
          name: 'storage__containerEndpoint'
          value: sharedStorageAccount.properties.primaryEndpoints.blob
        }
        {
          name: 'storage__containerName'
          value: responsesContainerName
        }
        {
          name: 'ConfigurationServiceEndpoint'
          value: appConfigurationService.properties.endpoint
        }
      ]
    }
  }
}

output functionName string = function.name
