param prefix string
param location string = resourceGroup().location

module csharpWindows '../../../Deployment/testFunctions/parts/windowsFunction.bicep' = {
  name: '${deployment().name}-Windows'
  params: {
    title: 'Azure Windows - Node'
    functionName: '${prefix}nodewin'
    workerRuntime: 'node'
    language: 'Nodejs'
    location: location
    runtimeVersion: '~4'
    prefix: prefix
    registrationName: 'AzureNodeWin' // Windows is blocked by microsoft since it is a reserved word 🤯
    testPath: '/api/Test'
    sortOrder: 200
    fxVersion: 'node|20'
  }
}

module csharpLinux '../../../Deployment/testFunctions/parts/linuxFunction.bicep' = {
  name: '${deployment().name}-Linux'
  dependsOn: [
    csharpWindows // we need to deploy one by one to not overload the device configuration service
  ]
  params: {
    title: 'Azure Linux - Node'
    functionName: '${prefix}nodelin'
    workerRuntime: 'node'
    language: 'Nodejs'
    location: location
    runtimeVersion: '~4'
    prefix: prefix
    registrationName: 'AzureNodeLinux'
    testPath: '/api/Test'
    sortOrder: 205
    fxVersion: 'node|20'
  }
}

module csharpLinuxFlex '../../../Deployment/testFunctions/parts/linuxFunctionFlex.bicep' = {
  name: '${deployment().name}-LinuxFlex'
  dependsOn: [
    csharpWindows // we need to deploy one by one to not overload the device configuration service
  ]
  params: {
    title: 'Azure Flex Consumption - Node'
    functionName: '${prefix}nodeflx'
    workerRuntime: 'node'
    language: 'Nodejs'
    sku: 'flex'
    // Not yet available in west europe
    location: 'northeurope'
    runtimeVersion: '~4'
    prefix: prefix
    registrationName: 'AzureNodeLinux'
    testPath: '/api/Test'
    sortOrder: 205
    functionAppRuntimeVersion: '20'
  }
}


