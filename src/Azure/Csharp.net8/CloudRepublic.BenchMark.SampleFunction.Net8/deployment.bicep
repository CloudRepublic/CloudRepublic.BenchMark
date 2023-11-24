param prefix string
param location string = resourceGroup().location

module csharpWindows '../../../../Deployment/testFunctions/parts/windowsFunction.bicep' = {
  name: '${deployment().name}-csharpWindows'
  params: {
    title: 'Azure Windows - C# .NET 8'
    functionName: '${prefix}csharpnet8win'
    workerRuntime: 'dotnet-isolated'
    language: 'Csharp'
    location: location
    runtimeVersion: '~4'
    sku: 'net8'
    prefix: prefix
    registrationName: 'AzureCsharpWin' // Windows is blocked by microsoft since it is a reserved word 🤯
    testPath: '/api/Trigger'
    sortOrder: 100
    fxVersion: 'dotnet|6.0'
  }
}

module csharpLinux '../../../../Deployment/testFunctions/parts/linuxFunction.bicep' = {
  name: '${deployment().name}-csharpLinux'
  dependsOn: [
    csharpWindows // we need to deploy one by one to not overload the device configuration service
  ]
  params: {
    title: 'Azure Linux - C# .NET 8'
    functionName: '${prefix}csharpnet8lin'
    workerRuntime: 'dotnet-isolated'
    language: 'Csharp'
    location: location
    runtimeVersion: '~4'
    sku: 'net8'
    prefix: prefix
    registrationName: 'AzureCsharpLinux'
    testPath: '/api/Trigger'
    sortOrder: 105
    fxVersion: 'dotnet|6.0'
  }
}
