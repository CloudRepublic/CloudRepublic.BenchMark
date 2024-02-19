// global
param location string

// resources
param storageAccountName string

resource storageAccount 'Microsoft.Storage/storageAccounts@2021-04-01' = {
  name: storageAccountName
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

  resource blobServices 'blobServices' = {
    name: 'default'

    resource container 'containers' = {
      name: 'responses'
    }
  }

  resource tableServices 'tableServices' = {
    name: 'default'

    resource resultsTable 'tables' = {
      name: 'results'
    }
  }
}

output storageName string = storageAccount.name
output resultsTableName string = storageAccount::tableServices::resultsTable.name
output responsesContainerName string = storageAccount::blobServices::container.name
