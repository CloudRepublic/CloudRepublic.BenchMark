param prefix string
param registrationName string
param functionName string
param location string
param testPath string

@allowed(['dotnet', 'dotnet-isolated', 'node', 'java', 'powershell', 'python'])
param workerRuntime string

@allowed(['Csharp', 'Nodejs', 'Python', 'Java', 'Fsharp'])
param language string

@allowed(['~4'])
param runtimeVersion string

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
}


resource functionFarm 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: '${functionName}plan'
  location: location
  kind: 'functionapp'
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
    size: 'Y1'
    family: 'Y'
    capacity: 0
  }
}

resource function 'Microsoft.Web/sites@2022-03-01' = {
  name: functionName
  location: location
  kind: 'functionapp'
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
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAccount.listKeys().keys[0].value}'
        }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: workerRuntime
        }
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: runtimeVersion
        }
        {
          name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAccount.listKeys().keys[0].value}'
        }
      ]
    }
  }
}

var defaultHostKey = listkeys('${function.id}/host/default', '2016-08-01').functionKeys.default
module configurationRegistration 'configurationRegistration.bicep' = {
  name: '${deployment().name}-configurationRegistration'
  params: {
    registrationName: registrationName
    prefix: prefix
    language: language
    runtime: 'FunctionsV4'
    hostEnvironment: 'Windows'
    authenticationHeaderName: 'x-functions-key'
    authenticationHeaderValue: defaultHostKey
    testEndpoint: 'https://${function.properties.defaultHostName}${testPath}'
  }
}
