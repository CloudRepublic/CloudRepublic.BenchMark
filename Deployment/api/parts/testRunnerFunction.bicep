param functionStorageAccountName string
param sharedStorageName string
param configServiceName string
param resultsTableName string
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

resource StorageTableDataContributorRole 'Microsoft.Authorization/roleDefinitions@2022-04-01' existing = {
  name: '0a9a7e1f-b9d0-4cc4-a60d-0319b160aaa3'
}

resource configServiceDataReaderRole 'Microsoft.Authorization/roleDefinitions@2022-04-01' existing = {
  name: '516239f1-63e1-4d78-a4de-a74fb236a071'
}

resource storageRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(functionStorageAccount.id, StorageTableDataContributorRole.id)
  scope: functionStorageAccount
  properties: {
    roleDefinitionId: StorageTableDataContributorRole.id
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
      windowsFxVersion: 'DOTNET|6.0'
      appSettings: [
        {
          name: 'AzureWebJobsStorage'
          value: 'DefaultEndpointsProtocol=https;AccountName=${functionStorageAccount.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${functionStorageAccount.listKeys().keys[0].value}'
        }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: 'dotnet'
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
          name: 'WEBSITE_ENABLE_SYNC_UPDATE_SITE'
          value: 'true'
        }
        {
          name: 'WEBSITE_RUN_FROM_PACKAGE'
          value: '1'
        }
        {
          name: 'storage__endpoint'
          value: sharedStorageAccount.properties.primaryEndpoints.table
        }
        {
          name: 'storage__resultsTableName'
          value: resultsTableName
        }
        {
          name: 'ConfigurationServiceEndpoint'
          value: appConfigurationService.properties.endpoint
        }
      ]
    }
  }
}
