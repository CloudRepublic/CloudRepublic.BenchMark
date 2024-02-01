param prefix string
param location string = resourceGroup().location

module csharpWindows '../../../../Deployment/testFunctions/parts/windowsFunction.bicep' = {
  name: '${deployment().name}-csharpWindows'
  params: {
    title: 'Azure Windows - C# .NET 8'
    functionName: '${prefix}cseightwin'
    workerRuntime: 'dotnet-isolated'
    language: 'Csharp'
    location: location
    runtimeVersion: '~4'
    sku: 'net8'
    prefix: prefix
    registrationName: 'AzureCsharpNet8Win' // Windows is blocked by microsoft since it is a reserved word 🤯
    testPath: '/api/Trigger'
    sortOrder: 111
    fxVersion: 'dotnet-isolated|8.0'
    use32BitWorkerProcess: false
  }
}

module csharpLinux '../../../../Deployment/testFunctions/parts/linuxFunction.bicep' = {
  name: '${deployment().name}-csharpLinux'
  dependsOn: [
    csharpWindows // we need to deploy one by one to not overload the device configuration service
  ]
  params: {
    title: 'Azure Linux - C# .NET 8'
    functionName: '${prefix}cseightlin'
    workerRuntime: 'dotnet-isolated'
    language: 'Csharp'
    location: location
    runtimeVersion: '~4'
    sku: 'net8'
    prefix: prefix
    registrationName: 'AzureCsharpNet8Linux'
    testPath: '/api/Trigger'
    sortOrder: 116
    fxVersion: 'dotnet-isolated|8.0'
  }
}
