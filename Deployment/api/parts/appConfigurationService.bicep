param prefix string
param location string

resource keyVault 'Microsoft.KeyVault/vaults@2022-07-01' = {
  name: '${prefix}vault'
  location: location
  properties: {
    sku: {
      name: 'standard'
      family: 'A'
    }
    tenantId: subscription().tenantId
    accessPolicies: []
  }

  resource accessPolicies 'accessPolicies' = {
    name: 'add'
    properties: {
      accessPolicies: [
        {
          tenantId: appConfigurationService.identity.tenantId
          objectId: appConfigurationService.identity.principalId
          permissions: {
            secrets: [
              'get'
            ]
          }
        }
      ]
    }
  }
}

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
