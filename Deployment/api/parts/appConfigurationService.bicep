param prefix string
param location string

// config will be partely done by hand
resource appConfigurationService 'Microsoft.AppConfiguration/configurationStores@2022-05-01' = {
    name: '${prefix}config'
    location: location
    identity: {
      type: 'SystemAssigned'
    }
    sku: {
      name: 'Free'
    }
    properties: {
        publicNetworkAccess: 'Enabled'
    }
}

output appConfigurationServiceName string = appConfigurationService.name
