param location string = resourceGroup().location
param prefix string

module api 'api/deployment.bicep' = {
  name: '${deployment().name}-api'
  params: {
    location: location
    prefix: prefix
  }
}

module frontend 'frontend/staticWebApp.bicep' = {
  name: '${deployment().name}-frontend'
  params: {
    location: location
    apiFunctionName: api.outputs.apiFunctionName
    prefix: prefix
  }
}
