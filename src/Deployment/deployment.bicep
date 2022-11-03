param location string = resourceGroup().location
param prefix string

module testFunctions 'testFunctions/deployment.bicep' = {
  name: 'testFunctions'
  params: {
    location: location
    prefix: prefix
  }
}

module api 'api/deployment.bicep' = {
  name: 'api'
  params: {
    location: location
    prefix: prefix
  }
}
