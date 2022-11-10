param prefix string
param location string = resourceGroup().location

var csharpWindowsName = '${prefix}csharpwin'
module csharpWindows '../../../../Deployment/testFunctions/parts/windowsFunction.bicep' = {
  name: '${deployment().name}-csharpWindows'
  params: {
    functionName: csharpWindowsName
    workerRuntime: 'dotnet'
    language: 'Csharp'
    location: location
    runtimeVersion: '~4'
    prefix: prefix
    registrationName: 'AzureCsharpWin' // Windows is blocked by microsoft since it is a reserved word 🤯
    testPath: '/api'
  }
}

var csharpLinuxName = '${prefix}csharplin'
module csharpLinux '../../../../Deployment/testFunctions/parts/linuxFunction.bicep' = {
  name: '${deployment().name}-csharpLinux'
  dependsOn: [
    csharpWindows // we need to deploy one by one to not overload the device configuration service
  ]
  params: {
    functionName: csharpLinuxName
    workerRuntime: 'dotnet'
    language: 'Csharp'
    location: location
    runtimeVersion: '~4'
    prefix: prefix
    registrationName: 'AzureCsharpLinux'
    testPath: '/api'
  }
}
