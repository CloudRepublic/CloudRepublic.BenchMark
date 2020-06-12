$username = "#{{USERNAME}}#";
$password = "#{{PASSWORD}}#";
$location = "West Europe";
$resourceGroupName = "BenchMark";
$resourceGroupLinuxName = "BenchMarkLinux";
$appInsightsName = "BenchMark-Insights";
$storageAccountName = "benchmark";
$storageAccountLinuxName = "benchmarklinux";
$sqlServerName = "sql-srv-benchMark";
$sqlServerAdminUsername = "benchmarkAdmin";
$sqlServerAdminPassword = "MySuperSecurePassword123!";
$sqlDatabaseName = "benchmark";
$redisCacheName = "BenchMark";
#$apimName = "BenchMark";
$consumptionLocation = "westeurope";
$orchestratorFunctionName = "BenchMark-Win-Orchestrator";

$windowsSampleFunctionCsharpName = "BenchMark-Sample-Win-Csharp";
$windowsSampleFunctionNodejsName = "BenchMark-Sample-Win-Nodejs";
$windowsSampleFunctionPythonName = "BenchMark-Sample-Win-Python";
$windowsSampleFunctionFsharpName = "BenchMark-Sample-Win-Fsharp";
$windowsSampleFunctionJavaName = "BenchMark-Sample-Win-Java";

$linuxSampleFunctionCsharpName = "BenchMark-Sample-Lin-Csharp";
$linuxSampleFunctionNodejsName = "BenchMark-Sample-Lin-Nodejs";
$linuxSampleFunctionPythonName = "BenchMark-Sample-Lin-Python";
$linuxSampleFunctionFsharpName = "BenchMark-Sample-Lin-Fsharp";
$linuxSampleFunctionJavaName = "BenchMark-Sample-Lin-Java";

$windowsSampleFunctionCsharpV3Name = "BenchMark-Sample-Win-Csharp-v3";
$windowsSampleFunctionNodejsV3Name = "BenchMark-Sample-Win-Nodejs-v3";
$windowsSampleFunctionFsharpV3Name = "BenchMark-Sample-Win-Fsharp-v3";
$windowsSampleFunctionJavaV3Name = "BenchMark-Sample-Win-Java-v3";

$linuxSampleFunctionCsharpV3Name = "BenchMark-Sample-Lin-Csharp-v3";
$linuxSampleFunctionNodejsV3Name = "BenchMark-Sample-Lin-Nodejs-v3";
$linuxSampleFunctionFsharpV3Name = "BenchMark-Sample-Lin-Fsharp-v3";
$linuxSampleFunctionJavaV3Name = "BenchMark-Sample-Lin-Java-v3";

$backendApiFunctionName = "BenchMark-Win-Api";
$cdnProfileName = "BenchMark";
$cdnEndpointName = "BenchMark";
$cdnCustomDomainName = "BenchMark";
$cdnCustomDomainHostname = "serverlessbenchmark.example.com";

#login into subscription
$subscription = az login -u $username -p $password

#create default resource-group
$resourceGroup = az group create --name $resourceGroupName --location $location | ConvertFrom-Json

#create linux resource-group
$resourceGroupLinux = az group create --location $location --name $resourceGroupLinuxName | ConvertFrom-Json

#create app-insights
$appInsights = az resource create --resource-group $resourceGroupName --resource-type "Microsoft.Insights/components" --name $appInsightsName --location $location --properties '{}' | ConvertFrom-Json

#create storage account windows
#static websites needs to be enable manually 
$storageAccount = az storage account create --resource-group $resourceGroupName --name $storageAccountName --sku Standard_LRS --https-only true --location $location --kind StorageV2 --access-tier Hot | ConvertFrom-Json

#create storage account windows
$storageAccountLinux = az storage account create --resource-group $resourceGroupLinuxName --name $storageAccountLinuxName --sku Standard_LRS --https-only true --location $location --kind StorageV2 --access-tier Hot | ConvertFrom-Json

#create sql server
$sqlServer = az sql server create --location $location --resource-group $resourceGroupName --name $sqlServerName --admin-user $sqlServerAdminUsername --admin-password $sqlServerAdminPassword | ConvertFrom-Json

#create sql database
$sqlDatabase = az sql db create --service-objective S0 --capacity 10 --name $sqlDatabaseName --server $sqlServerName --resource-group $resourceGroupName | ConvertFrom-Json

#create redis cache
$redisCache = az redis create --resource-group $resourceGroupName --name $redisCacheName --location $location --sku Basic --vm-size c0 | ConvertFrom-Json

#create backend api
$BackendApiFunc = az functionapp create --resource-group $resourceGroupName --name $backendApiFunctionName --storage-account $storageAccountName --consumption-plan-location  $consumptionLocation --runtime dotnet --os-type Windows --app-insights-key $appInsights.properties.InstrumentationKey | ConvertFrom-Json

#create function for orchestrator
$orchestratorFunc = az functionapp create --resource-group $resourceGroupName --name $orchestratorFunctionName --storage-account $storageAccountName --consumption-plan-location  $consumptionLocation --runtime dotnet --os-type Windows --app-insights-key $appInsights.properties.InstrumentationKey | ConvertFrom-Json

#Get function host-key for apim backend header
$auth = az account get-access-token | ConvertFrom-Json
$accessTokenHeader = @{ "Authorization" = "Bearer $($auth.accessToken)" }
$adminBearerTokenUri = "https://management.azure.com$($BackendApiFunc.id)/functions/admin/token?api-version=2016-08-01"
$adminBearerToken = Invoke-RestMethod -Method Get -Uri $adminBearerTokenUri -Headers $accessTokenHeader
$adminTokenHeader = @{ "Authorization" = "Bearer $($adminBearerToken)" }
$hostKeysUri = "https://$($BackendApiFunc.hostNames[0])/admin/host/keys/"
$hostKeys = Invoke-RestMethod -Method Get -Uri $hostKeysUri -Headers $adminTokenHeader

#create api management instance from template
$apim = az group deployment create --mode Incremental  --resource-group $resourceGroupName --template-file ../Deployment/Templates/apim.template.json --parameters location=$location sku='Consumption' publisherEmail='admin@example.com' customHostname=$cdnCustomDomainHostname publisherName='admin' apiFunctionId="$($BackendApiFunc.id)" apiFunctionName="$($BackendApiFunc.name)" apiFunctionDefaultHostname="$($BackendApiFunc.defaultHostName)" apiFunctionKey="$($hostKeys.keys.value)" redisName="$($redisCache.name)" redisKey="$($redisCache.accessKeys.primaryKey)" | ConvertFrom-Json

#V2 Windows triggers
#create windows sample function csharp
$sampleFuncWindowsCsharp = az functionapp create --resource-group $resourceGroupName --name $windowsSampleFunctionCsharpName --storage-account $storageAccountName --consumption-plan-location  $consumptionLocation --runtime dotnet --os-type Windows | ConvertFrom-Json --functions-version 2
#create windows sample function nodejs
$sampleFuncWindowsNodejs = az functionapp create --resource-group $resourceGroupName --name $windowsSampleFunctionNodejsName --storage-account $storageAccountName --consumption-plan-location  $consumptionLocation --runtime node --os-type Windows | ConvertFrom-Json  --functions-version 2
#create windows sample function fsharp
$sampleFuncWindowsFsharp = az functionapp create --resource-group $resourceGroupName --name $windowsSampleFunctionFsharpName --storage-account $storageAccountName --consumption-plan-location  $consumptionLocation --runtime dotnet --os-type Windows | ConvertFrom-Json --functions-version 2
#create windows sample function python
$sampleFuncWindowsPython = az functionapp create --resource-group $resourceGroupName --name $windowsSampleFunctionPythonName --storage-account $storageAccountName --consumption-plan-location  $consumptionLocation --runtime python --runtime-version 3.7 --os-type Windows | ConvertFrom-Json --functions-version 2
#create windows sample function Java
$sampleFuncWindowsJava = az functionapp create --resource-group $resourceGroupName --name $windowsSampleFunctionJavaName --storage-account $storageAccountName --consumption-plan-location  $consumptionLocation --runtime dotnet --os-type Windows | ConvertFrom-Json  --functions-version 2

#V2 Linux triggers
#create linux sample function csharp
$sampleFuncLinuxCsharp = az functionapp create --resource-group $resourceGroupLinuxName --name $linuxSampleFunctionCsharpName --storage-account $storageAccountLinuxName --consumption-plan-location  $consumptionLocation --runtime dotnet --os-type Linux | ConvertFrom-Json --functions-version 2
#create linux sample function nodejs
$sampleFuncLinuxNodejs = az functionapp create --resource-group $resourceGroupLinuxName --name $linuxSampleFunctionNodejsName --storage-account $storageAccountLinuxName --consumption-plan-location  $consumptionLocation --runtime node --os-type Linux | ConvertFrom-Json --functions-version 2
#create linux sample function fsharp
$sampleFuncLinuxFsharp = az functionapp create --resource-group $resourceGroupLinuxName --name $linuxSampleFunctionFsharpName --storage-account $storageAccountLinuxName --consumption-plan-location  $consumptionLocation --runtime dotnet --os-type Linux | ConvertFrom-Json --functions-version 2
#create linux sample function Java
$sampleFuncLinuxJava = az functionapp create --resource-group $resourceGroupLinuxName --name $linuxSampleFunctionJavaName --storage-account $storageAccountLinuxName --consumption-plan-location  $consumptionLocation --runtime dotnet --os-type Linux | ConvertFrom-Json --functions-version 2
#create Linux sample function python
$sampleFuncLinuxPython = az functionapp create --resource-group $resourceGroupLinuxName --name $linuxSampleFunctionPythonName --storage-account $storageAccountLinuxName --consumption-plan-location  $consumptionLocation --runtime python --runtime-version 3.7 --os-type Linux | ConvertFrom-Json --functions-version 2

#V3 Windows triggers
#create windows sample function V3 csharp
$sampleFuncWindowsCsharpv3 = az functionapp create --resource-group $resourceGroupName --name $windowsSampleFunctionCsharpV3Name --storage-account $storageAccountName --consumption-plan-location  $consumptionLocation --runtime dotnet --os-type Windows | ConvertFrom-Json --functions-version 3
#create windows sample function V3 nodejs
$sampleFuncV3WindowsNodejsv3 = az functionapp create --resource-group $resourceGroupName --name $windowsSampleFunctionNodejsV3Name --storage-account $storageAccountName --consumption-plan-location  $consumptionLocation --runtime node --os-type Windows | ConvertFrom-Json --functions-version 3
#create windows sample function fsharp
$sampleFuncWindowsFsharpV3 = az functionapp create --resource-group $resourceGroupName --name $windowsSampleFunctionFsharpV3Name --storage-account $storageAccountName --consumption-plan-location  $consumptionLocation --runtime dotnet --os-type Windows | ConvertFrom-Json --functions-version 3
#create windows sample function Java
$sampleFuncWindowsJavaV3 = az functionapp create --resource-group $resourceGroupName --name $windowsSampleFunctionJavaV3Name --storage-account $storageAccountName --consumption-plan-location  $consumptionLocation --runtime dotnet --os-type Windows | ConvertFrom-Json  --functions-version 3


#V3 Linux triggers
#create linux sample function V3 csharp
$sampleFuncV3LinuxCsharpv3 = az functionapp create --resource-group $resourceGroupLinuxName --name $linuxSampleFunctionCsharpV3Name --storage-account $storageAccountLinuxName --consumption-plan-location  $consumptionLocation --runtime dotnet --os-type Linux | ConvertFrom-Json --functions-version 3
#create linux sample function V3 nodejs
$sampleFuncV3LinuxNodejsv3 = az functionapp create --resource-group $resourceGroupLinuxName --name $linuxSampleFunctionNodejsV3Name --storage-account $storageAccountLinuxName --consumption-plan-location  $consumptionLocation --runtime node --os-type Linux | ConvertFrom-Json --functions-version 3
#create Linux sample function fsharp
$sampleFuncWLinuxFsharpV3 = az functionapp create --resource-group $resourceGroupLinuxName --name $linuxSampleFunctionFsharpV3Name --storage-account $storageAccountName --consumption-plan-location  $consumptionLocation --runtime dotnet --os-type Linux | ConvertFrom-Json --functions-version 3
#create Linux sample function Java
$sampleFuncLinuxJavaV3 = az functionapp create --resource-group $resourceGroupLinuxName --name $linuxSampleFunctionJavaV3Name --storage-account $storageAccountName --consumption-plan-location  $consumptionLocation --runtime dotnet --os-type Linux | ConvertFrom-Json  --functions-version 3



#create cdn profile
$cdnProfile = az cdn profile create --location $location --resource-group $resourceGroupName --name $cdnProfileName --sku Standard_Microsoft

#create cdn endpoint
$originHostname = $storageAccount.primaryEndpoints.web -replace "https://", "" -replace "/", "";
$cdnEndpoint = az cdn endpoint create --location $location --resource-group $resourceGroupName --profile-name $cdnProfileName --name $cdnEndpointName --origin $originHostname --origin-host-header $originHostname --enable-compression true --query-string-caching IgnoreQueryString

#create custom domain - Enable only if DNS is in place!
#$cdnCustomDomain = az cdn custom-domain create --location $location --resource-group $resourceGroupName --profile-name $cdnProfileName --endpoint-name $cdnEndpointName --name $cdnCustomDomainName --hostname $cdnCustomDomainHostname
#$cdnCustomDomain = az cdn custom-domain enable-https --resource-group $resourceGroupName --profile-name $cdnProfileName --endpoint-name $cdnEndpointName --name $cdnCustomDomainName