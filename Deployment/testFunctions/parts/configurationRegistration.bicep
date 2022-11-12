param prefix string

param registrationName string

@allowed(['Csharp', 'Nodejs', 'Python', 'Java', 'Fsharp'])
param language string

@allowed(['FunctionsV4', 'Firebase'])
param runtime string

@allowed(['Linux', 'Windows'])
param hostEnvironment string

param title string
param testEndpoint string
param authenticationHeaderName string
param authenticationHeaderValue string
param sku string = ''
param sortOrder int

param sentinelValue string = utcNow()

resource configService 'Microsoft.AppConfiguration/configurationStores@2022-05-01' existing = {
  name: '${prefix}config'

  resource LanguageKey 'keyValues' = {
    name: 'BenchMarkTests:${registrationName}:Language'
    properties: {
      value: language
    }
  }

  resource Title 'keyValues' = {
    name: 'BenchMarkTests:${registrationName}:Title'
    properties: {
      value: title
    }
  }

  resource Runtime 'keyValues' = {
    name: 'BenchMarkTests:${registrationName}:Runtime'
    properties: {
      value: runtime
    }
  }

  resource CloudProvider 'keyValues' = {
    name: 'BenchMarkTests:${registrationName}:CloudProvider'
    properties: {
      value: 'Azure'
    }
  }

  resource HostEnvironment 'keyValues' = {
    name: 'BenchMarkTests:${registrationName}:HostEnvironment'
    properties: {
      value: hostEnvironment
    }
  }

  resource TestEndpoint 'keyValues' = {
    name: 'BenchMarkTests:${registrationName}:TestEndpoint'
    properties: {
      value: testEndpoint
    }
  }

  resource AuthenticationHeaderName 'keyValues' = {
    name: 'BenchMarkTests:${registrationName}:AuthenticationHeaderName'
    properties: {
      value: authenticationHeaderName
    }
  }

  resource AuthenticationHeaderValue 'keyValues' = {
    name: 'BenchMarkTests:${registrationName}:AuthenticationHeaderValue'
    properties: {
      value: authenticationHeaderValue
    }
  }

  resource Sku 'keyValues' = {
    name: 'BenchMarkTests:${registrationName}:Sku'
    properties: {
      value: sku
    }
  }

  resource Sentinel 'keyValues' = {
    name: 'BenchMarkTests:Sentinel'
    properties: {
      value: sentinelValue
    }
  }
}
