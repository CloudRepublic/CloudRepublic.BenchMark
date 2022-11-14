param prefix string
param location string = resourceGroup().location

module csharpWindows '../../../Deployment/testFunctions/parts/windowsFunction.bicep' = {
  name: '${deployment().name}-Windows'
  params: {
    title: 'Azure - Node - Windows'
    functionName: '${prefix}nodewin'
    workerRuntime: 'node'
    language: 'Nodejs'
    location: location
    runtimeVersion: '~4'
    prefix: prefix
    registrationName: 'AzureNodeWin' // Windows is blocked by microsoft since it is a reserved word 🤯
    testPath: '/api/Test'
    sortOrder: 200
    fxVersion: 'node|16'
  }
}

module csharpLinux '../../../Deployment/testFunctions/parts/linuxFunction.bicep' = {
  name: '${deployment().name}-Linux'
  dependsOn: [
    csharpWindows // we need to deploy one by one to not overload the device configuration service
  ]
  params: {
    title: 'Azure - C# - Linux'
    functionName: '${prefix}nodelin'
    workerRuntime: 'node'
    language: 'Nodejs'
    location: location
    runtimeVersion: '~4'
    prefix: prefix
    registrationName: 'AzureNodeLinux'
    testPath: '/api/Test'
    sortOrder: 205
    fxVersion: 'node|16'
  }
}
