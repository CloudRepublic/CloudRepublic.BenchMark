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
    registrationName: 'AzureCsharpWindows'
    testPath: '/'
  }
}

var csharpLinuxName = '${prefix}csharplin'
module csharpLinux '../../../../Deployment/testFunctions/parts/linuxFunction.bicep' = {
  name: '${deployment().name}-csharpLinux'
  params: {
    functionName: csharpLinuxName
    workerRuntime: 'dotnet'
    language: 'Csharp'
    location: location
    runtimeVersion: '~4'
    prefix: prefix
    registrationName: 'AzureCsharpLinux'
    testPath: '/'
  }
}
