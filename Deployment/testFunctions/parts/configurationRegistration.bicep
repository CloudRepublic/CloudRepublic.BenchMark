param prefix string

param registrationName string

@allowed(['Csharp', 'Nodejs', 'Python', 'Java', 'Fsharp'])
param language string

@allowed(['FunctionsV4', 'Firebase'])
param runtime string

@allowed(['Linux', 'Windows'])
param hostEnvironment string

param testEndpoint string
param authenticationHeaderName string
param authenticationHeaderValue string

resource configService 'Microsoft.AppConfiguration/configurationStores@2022-05-01' existing = {
  name: '${prefix}config'

  resource LanguageKey 'keyValues' = {
    name: 'BenchMarkTests:${registrationName}:Language'
    properties: {
      value: language
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

  resource Sentinel 'keyValues' = {
    name: 'BenchMarkTests:Sentinel'
    properties: {
      value: deployment().name
    }
  }
}
