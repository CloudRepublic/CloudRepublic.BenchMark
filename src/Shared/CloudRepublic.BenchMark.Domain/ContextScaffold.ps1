param(
    [Parameter(Mandatory=$true)][string]$username,
    [Parameter(Mandatory=$true)][string]$password,
    [Parameter(Mandatory=$true)][string]$datasource,
    [Parameter(Mandatory=$true)][string]$catalog,
    [Parameter(Mandatory=$true)][string]$contextName
)

$workingDir = $PSScriptRoot;

Write-Host "Project dir= $workingDir";
Write-Host "Creating temp project to run scaffold on";
cd ..
mkdir temp
cd temp
dotnet new web
Write-Host "Done creating temp project";
Write-Host "Scaffolding dbcontext into project";
dotnet ef dbcontext scaffold "data source=$($datasource);initial catalog=$($catalog);user id=$($username);password=$($password);MultipleActiveResultSets=True;App=EntityFramework" Microsoft.EntityFrameworkCore.SqlServer --output-dir Entities --context $contextName --project temp.csproj
Write-Host "Done scaffolding dbcontext into temp project";
New-Item -ErrorAction Ignore -ItemType directory -Path "$workingDir/Entities";
Move-Item ./Entities/* "$workingDir/Entities" -force;
Write-Host "Scaffold completed! Starting clean-up";
cd ..
Remove-Item ./temp -force -Recurse;
cd $workingDir;
Write-Host "Clean-up completed!";