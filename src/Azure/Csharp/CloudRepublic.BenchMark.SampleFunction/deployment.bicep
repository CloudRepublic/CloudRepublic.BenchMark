param prefix string
param location string = resourceGroup().location

module csharpWindows '../../../../Deployment/testFunctions/parts/windowsFunction.bicep' = {
  name: '${deployment().name}-csharpWindows'
  params: {
    title: 'Azure Windows - C# .NET 6 (inprocess)'
    functionName: '${prefix}csharpwin'
    workerRuntime: 'dotnet'
    language: 'Csharp'
    location: location
    runtimeVersion: '~4'
    prefix: prefix
    registrationName: 'AzureCsharpWin' // Windows is blocked by microsoft since it is a reserved word 🤯
    testPath: '/api/Trigger'
    sortOrder: 100
    fxVersion: 'dotnet|6.0'
    use32BitWorkerProcess: false
  }
}

module csharpLinux '../../../../Deployment/testFunctions/parts/linuxFunction.bicep' = {
  name: '${deployment().name}-csharpLinux'
  dependsOn: [
    csharpWindows // we need to deploy one by one to not overload the device configuration service
  ]
  params: {
    title: 'Azure Linux - C# .NET 6 (inprocess)'
    functionName: '${prefix}csharplin'
    workerRuntime: 'dotnet'
    language: 'Csharp'
    location: location
    runtimeVersion: '~4'
    prefix: prefix
    registrationName: 'AzureCsharpLinux'
    testPath: '/api/Trigger'
    sortOrder: 105
    fxVersion: 'dotnet|6.0'
  }
}
