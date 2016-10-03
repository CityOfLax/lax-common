if(Test-Path .\artifacts) { Remove-Item .\artifacts -Force -Recurse }

EnsurePsbuildInstalled

exec { & dotnet restore }

Invoke-MSBuild

$revision = @{ $true = $env:APPVEYOR_BUILD_NUMBER; $false = 1 }[$env:APPVEYOR_BUILD_NUMBER -ne $NULL];
$revision = "{0:D4}" -f [convert]::ToInt32($revision, 10)

$packagesToPublish = @("Lax.Data", "Lax.Data.EntityFramework")

foreach ($packageToPublish in $packagesToPublish) {
	exec { & dotnet pack ".\src\" + $packagesToPublish -c Release -o .\artifacts --version-suffix=$revision }  
}

