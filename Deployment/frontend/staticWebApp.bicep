param prefix string
param location string
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

  resource api 'linkedBackends' = {
    name: 'api'
    properties: {
      backendResourceId: apiFunction.id
    }
  }
}
