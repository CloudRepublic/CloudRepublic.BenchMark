
param prefix string
param location string = resourceGroup().location

var apiFunctionName = '${prefix}api'
var testRunnerFunctionName = '${prefix}testrunner'
var sharedStorageName = '${prefix}shared'

module sharedStorage 'parts/sharedStorage.bicep' = {
  name: '${deployment().name}-sharedfuncstor'
  params: {
    storageAccountName: sharedStorageName
    location: location
  }
}

module apiFunctionStorage 'parts/storage.bicep' = {
  name: '${deployment().name}-apifuncstor'
  params: {
    storageAccountName: '${apiFunctionName}stor'
    location: location
  }
}

module testRunnerStorage 'parts/storage.bicep' = {
  name: '${deployment().name}-testrunnerfuncstor'
  params: {
    storageAccountName: '${testRunnerFunctionName}stor'
    location: location
  }
}

module configService 'parts/appConfigurationService.bicep' = {
  name: '${deployment().name}-config'
  params: {
    prefix: prefix
    location: location
  }
}

module apiFunction 'parts/apiFunction.bicep' = {
  name: '${deployment().name}-apifunc'
  params: {
    functionName: apiFunctionName
    location: location
    functionStorageAccountName: apiFunctionStorage.outputs.storageName
    appInsightsName: apiFunctionName
    sharedStorageName: sharedStorage.outputs.storageName
    resultsTableName: sharedStorage.outputs.resultsTableName
    configServiceName: configService.outputs.appConfigurationServiceName
  }
}

module testRunnerFunction 'parts/testRunnerFunction.bicep' = {
  name: '${deployment().name}-testrunnerfunc'
  params: {
    functionName: testRunnerFunctionName
    location: location
    functionStorageAccountName: testRunnerStorage.outputs.storageName
    appInsightsName: testRunnerFunctionName
    sharedStorageName: sharedStorage.outputs.storageName
    resultsTableName: sharedStorage.outputs.resultsTableName
    configServiceName: configService.outputs.appConfigurationServiceName
  }
}


output apiFunctionName string = apiFunction.outputs.functionName
