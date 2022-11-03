param prefix string
param location string

var csharpWindowsName = '${prefix}csharpwin'
module csharpWindows 'parts/windowsFunction.bicep' = {
  name: '${deployment().name}-csharpWindows'
  params: {
    functionName: csharpWindowsName
    language: 'dotnet'
    location: location
    runtimeVersion: '~4'
  }
}

var csharpLinuxName = '${prefix}csharplin'
module csharpLinux 'parts/linuxFunction.bicep' = {
  name: '${deployment().name}-csharpLinux'
  params: {
    functionName: csharpLinuxName
    language: 'dotnet'
    location: location
    runtimeVersion: '~4'
  }
}

var fsharpWindowsName = '${prefix}fsharpwin'
module fsharpWindows 'parts/windowsFunction.bicep' = {
  name: '${deployment().name}-fsharpWindows'
  params: {
    functionName: fsharpWindowsName
    language: 'dotnet'
    location: location
    runtimeVersion: '~4'
  }
}

var fsharpLinuxName = '${prefix}fsharplin'
module fsharpLinux 'parts/linuxFunction.bicep' = {
  name: '${deployment().name}-fsharpLinux'
  params: {
    functionName: fsharpLinuxName
    language: 'dotnet'
    location: location
    runtimeVersion: '~4'
  }
}

var javaWindowsName = '${prefix}javawin'
module javaWindows 'parts/windowsFunction.bicep' = {
  name: '${deployment().name}-javaWindows'
  params: {
    functionName: javaWindowsName
    language: 'java'
    location: location
    runtimeVersion: '~4'
  }
}

var javaLinuxName = '${prefix}javalin'
module javaLinux 'parts/linuxFunction.bicep' = {
  name: '${deployment().name}-javaLinux'
  params: {
    functionName: javaLinuxName
    language: 'java'
    location: location
    runtimeVersion: '~4'
  }
}

var nodeWindowsName = '${prefix}nodewin'
module nodeWindows 'parts/windowsFunction.bicep' = {
  name: '${deployment().name}-nodeWindows'
  params: {
    functionName: nodeWindowsName
    language: 'node'
    location: location
    runtimeVersion: '~4'
  }
}

var nodeLinuxName = '${prefix}nodelin'
module nodeLinux 'parts/linuxFunction.bicep' = {
  name: '${deployment().name}-nodeLinux'
  params: {
    functionName: nodeLinuxName
    language: 'node'
    location: location
    runtimeVersion: '~4'
  }
}

var pythonWindowsName = '${prefix}pytwin'
module pythonWindows 'parts/windowsFunction.bicep' = {
  name: '${deployment().name}-pythonWindows'
  params: {
    functionName: pythonWindowsName
    language: 'python'
    location: location
    runtimeVersion: '~4'
  }
}

var pythonLinuxName = '${deployment().name}pytlin'
module pythonLinux 'parts/linuxFunction.bicep' = {
  name: '${deployment().name}-pythonLinux'
  params: {
    functionName: pythonLinuxName
    language: 'python'
    location: location
    runtimeVersion: '~4'
  }
}

output functionNames array = [
  csharpWindowsName
  csharpLinuxName
  fsharpWindowsName
  fsharpLinuxName
  javaWindowsName
  javaLinuxName
  nodeWindowsName
  nodeLinuxName
  pythonWindowsName
  pythonLinuxName
]
