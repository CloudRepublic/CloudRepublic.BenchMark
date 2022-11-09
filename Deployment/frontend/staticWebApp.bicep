param prefix string
param location string = resourceGroup().location
param apiFunctionName string

resource apiFunction 'Microsoft.Web/sites@2022-03-01' existing = {
  name: apiFunctionName
}

resource staticWebApp 'Microsoft.Web/staticSites@2022-03-01' = {
  name: '${prefix}frontend'
  location: location
  sku: {
    name: 'Standard'
    tier: 'Standard'
  }
  properties: {
    allowConfigFileUpdates: true
  }

  resource api 'linkedBackends' = {
    name: 'backend1'
    properties: {
      backendResourceId: apiFunction.id
      region: apiFunction.location
    }
  }
}
