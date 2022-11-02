
param apiFunctionName string
param testRunnerFunctionName string
param location string = resourceGroup().location

module apiFunctionStorage 'parts/storage.bicep' = {
  name: '${deployment().name}-funcstor'
  params: {
    storageAccountName: '${apiFunctionName}stor'
    location: location
  }
}

module testRunnerStorage 'parts/storage.bicep' = {
  name: '${deployment().name}-funcstor'
  params: {
    storageAccountName: '${testRunnerFunctionName}stor'
    location: location
  }
}

module apiFunction 'parts/apiFunction.bicep' = {
  name: '${deployment().name}-func'
  params: {
    functionName: apiFunctionName
    location: location
    functionStorageAccountName: apiFunctionStorage.outputs.storageName
    appInsightsName: apiFunctionName
  }
}

module testRunnerFunction 'parts/testRunnerFunction.bicep' = {
  name: '${deployment().name}-func'
  params: {
    functionName: testRunnerFunctionName
    location: location
    functionStorageAccountName: testRunnerStorage.outputs.storageName
    appInsightsName: testRunnerFunctionName
  }
}
