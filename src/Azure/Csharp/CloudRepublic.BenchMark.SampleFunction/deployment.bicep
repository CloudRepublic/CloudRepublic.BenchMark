param prefix string
param location string

var csharpWindowsName = '${prefix}csharpwin'
module csharpWindows '../../../../Deployment/testFunctions/parts/windowsFunction.bicep' = {
  name: '${deployment().name}-csharpWindows'
  params: {
    functionName: csharpWindowsName
    language: 'dotnet'
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
    language: 'dotnet'
    location: location
    runtimeVersion: '~4'
    prefix: prefix
    registrationName: 'AzureCsharpLinux'
    testPath: '/'
  }
}
