[CmdletBinding()]
param($TestSecret)

$secret = "$(TestSecret)"

Write-Host "Starting deployment"

Write-Host "secret: $($secret)"
Write-Host "testSecret: $($TestSecret)"

Write-Host "Finished deployment"
