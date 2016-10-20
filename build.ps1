<#  
.SYNOPSIS
    You can add this to you build script to ensure that psbuild is available before calling
    Invoke-MSBuild. If psbuild is not available locally it will be downloaded automatically.
#>
function EnsurePsbuildInstalled{  
    [cmdletbinding()]
    param(
        [string]$psbuildInstallUri = 'https://raw.githubusercontent.com/ligershark/psbuild/master/src/GetPSBuild.ps1'
    )
    process{
        if(-not (Get-Command "Invoke-MsBuild" -errorAction SilentlyContinue)){
            'Installing psbuild from [{0}]' -f $psbuildInstallUri | Write-Verbose
            (new-object Net.WebClient).DownloadString($psbuildInstallUri) | iex
        }
        else{
            'psbuild already loaded, skipping download' | Write-Verbose
        }

        # make sure it's loaded and throw if not
        if(-not (Get-Command "Invoke-MsBuild" -errorAction SilentlyContinue)){
            throw ('Unable to install/load psbuild from [{0}]' -f $psbuildInstallUri)
        }
    }
}

# Taken from psake https://github.com/psake/psake

<#  
.SYNOPSIS
  This is a helper function that runs a scriptblock and checks the PS variable $lastexitcode
  to see if an error occcured. If an error is detected then an exception is thrown.
  This function allows you to run command-line programs without having to
  explicitly check the $lastexitcode variable.
.EXAMPLE
  exec { svn info $repository_trunk } "Error executing SVN. Please verify SVN command-line client is installed"
#>
function Exec  
{
    [CmdletBinding()]
    param(
        [Parameter(Position=0,Mandatory=1)][scriptblock]$cmd,
        [Parameter(Position=1,Mandatory=0)][string]$errorMessage = ($msgs.error_bad_command -f $cmd)
    )
    & $cmd
    if ($lastexitcode -ne 0) {
        throw ("Exec: " + $errorMessage)
    }
}


if(Test-Path .\artifacts) { Remove-Item .\artifacts -Force -Recurse }

EnsurePsbuildInstalled

exec { & dotnet restore }

Invoke-MSBuild

$revision = @{ $true = $env:APPVEYOR_BUILD_NUMBER; $false = 1 }[$env:APPVEYOR_BUILD_NUMBER -ne $NULL];
$revision = "CI{0:D4}" -f [convert]::ToInt32($revision, 10)

$packagesToPublish = @(
    "Lax.Data", 
    "Lax.Data.Dapper",
    "Lax.Data.EntityFramework",
    "Lax.Data.EntityFrameworkCore",
    "Lax.Caching.Memory.Namespaced",
    "Lax.Caching.Memory.Typed",
    "Lax.Console.Hosting",
    "Lax.DirectoryServices",
    "Lax.Helpers.Booleans",
    "Lax.Helpers.CryptographicRandomStrings",
    "Lax.Helpers.DateTimes",
    "Lax.Helpers.EffectiveDateEntity",
    "Lax.Helpers.EntityTypeConfigurations"
    "Lax.Helpers.EnumerationMapping",
    "Lax.Helpers.PasswordHashing",
    "Lax.Helpers.Reflection.Enumerations",
    "Lax.Helpers.Serialization.ByteArrays",
    "Lax.Helpers.Serialization.Xml",
    "Lax.Helpers.SoftlyDeletableEntity",
    "Lax.Helpers.StringComparison",
    "Lax.Helpers.StringManipulation",
    "Lax.Helpers.TimePeriods",
    "Lax.Jobs.Runner",
    "Lax.Jobs.Runner.Synchronous",
    "Lax.Jobs",
    "Lax.Jobs.Abstractions",
    "Lax.Mvc.AdminLte",
    "Lax.Mvc.Bootstrap",
    "Lax.Mvc.DataTables",
    "Lax.Security.Authentication",
    "Lax.Security.Authentication.CustomWindows",
    "Lax.Security.Authentication.CustomWindows.Ldap",
    "Lax.Security.Claims",
    "Lax.Security.Claims.Ldap",
    "Lax.TypeMapper",
    "Lax.TypeMapper.AutoMapper"
)

foreach ($packageToPublish in $packagesToPublish) {
    $packageProjectPath = ".\src\" + $packageToPublish
    exec { & dotnet pack $packageProjectPath -c Release -o .\artifacts --version-suffix=$revision }  
}

