########################################
#### Adapt to 32bit codebuild powershell
########################################

if ($PSHOME -like "*SysWOW64*")
{
  Write-Warning "Restarting this script under 64-bit Windows PowerShell."
  & (Join-Path ($PSHOME -replace "SysWOW64", "SysNative") powershell.exe) -File `
    (Join-Path $PSScriptRoot $MyInvocation.MyCommand) @args
  Exit $LastExitCode
}

# Was restart successful?
Write-Warning "Hello from $PSHOME"
Write-Warning "  (\SysWOW64\ = 32-bit mode, \System32\ = 64-bit mode)"
Write-Warning "Original arguments (if any): $args"


#Installing requirments

$OurHome = "C:\qbs-website"
if (!(Test-Path $OurHome -PathType Container)) {
  New-Item -ItemType Directory -Force Path $OurHome
}

$componentName = "VendorSystem"
$componentSubDir = "VendorSystem"

###############$$$$$$$$$#################$$$$$$#########
###### Install Choco, to enable vstools and Nuget

Set-ExecutionPolicy Bypass -Scope Process -Force; [System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072; iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))
$env:Path = [System.Environment]::GetEnvironmentVariable("Path","Machine") + ";" + [System.Environment]::GetEnvironmentVariable("Path","User") 

choco install NuGet.CommandLine; RefreshEnv; nuget restore -source "https://api.nuget.org/v3/index.json;https://www.myget.org/F/nuget" "C:\qbs-website\$componentName.sln"
choco install visualstudio2019-workload-webbuildtools -y;

#################################################
#### Add VS tools location and remove old version
#################################################

$fileName = "C:\qbs-website\$componentName\$componentSubDir.csproj"
$pattern = "Microsoft.CSharp.targets"
$Add = '<Import Project="C:\Program Files (x86)\Microsoft Visual Studio\2019\BuildTools\MSBuild\Microsoft\VisualStudio\v16.0\WebApplications\Microsoft.WebApplication.targets" />'

(Get-Content $fileName) | 
  Foreach-Object {
    $_ # send the current line to output
    if ($_ -match $pattern) 
    { 
      $Add
    }
  } | Set-Content $fileName 
  
Set-Content -Path $fileName -Value (Get-Content -path $fileName | Select-String -Pattern '<Import Project="$(MSBuildExtensionsPath32)\\Microsoft\\VisualStudio\\v10.0\\WebApplications\\Microsoft.WebApplication.targets" Condition="false" />' -NotMatch )

## Change Publish directory
$Target = '<PublishUrl>.*</PublishUrl>'
$newValue = '<PublishUrl>C:\Publish</PublishUrl>'
$file = 'C:\qbs-website\VendorSystem\Properties\PublishProfiles\FolderProfile.pubxml'
(Get-Content $file) -replace($Target, $newValue) | Set-Content $file


## Start build and publish
& "C:\Program Files (x86)\Microsoft Visual Studio\2019\BuildTools\MSBuild\Current\Bin\MSBuild.exe"  C:\qbs-website\$componentSubDir.sln /p:DeployOnBuild=true /p:PublishProfile=C:\qbs-website\$componentSubDir\Properties\PublishProfiles\FolderProfile.pubxml

if (!(Test-Path "C:\qbs-website\$componentSubDir\Reports" -PathType Container)){ 
  Copy-Item -Path "C:\qbs-website\$componentSubDir\Reports" -Destination "C:\Publish" -Recurse
}

##### Configre SQL connection
$ConfigFile = "C:\Publish\web.config"
$OldValues = '<add name="BayanEntities" connectionString=".* />'
$NewValues = '<add name="BayanEntities" connectionString="metadata=res://*/Models.Model1.Model1.csdl|res://*/Models.Model1.Model1.ssdl|res://*/Models.Model1.Model1.msl;          provider=System.Data.SqlClient;provider connection string=&quot;data source=terraform-20210622120633480900000005.cm0wa7goli8p.us-east-2.rds.amazonaws.com,1433;initial catalog=Vendor_QC;User Id=qbs_Adminp0wEr; Password=$*>9%ze3`8M3zETkKT;multipleactiveresultsets=True;application name=EntityFramework&quot;" providerName="System.Data.EntityClient" />' 

(Get-Content $ConfigFile) -replace($OldValues, $NewValues) | Set-Content $ConfigFile 

######
### IIS Implementation, creating site and binding
######

Import-Module WebAdministration  


Get-WebSite -Name "Default Web Site" -ErrorAction SilentlyContinue | Remove-WebSite -Confirm:$false -Verbose -ErrorAction SilentlyContinue
Remove-WebAppPool -Name "DefaultAppPool" -Confirm:$false -Verbose -ErrorAction SilentlyContinue

$iisAppPoolName = "qbs-website"  
$iisAppPoolDotNetVersion = "v4.0"  
  
$iisWebsiteFolderPath = "C:\Publish"  
$iisWebsiteName = "qbs-website"  
  
$iisWebsiteBindings = @(  
   @{protocol="http";bindingInformation="*:80:"}
)  
  
if (!(Test-Path IIS:\AppPools\$iisAppPoolName -pathType container))  
{  
New-Item IIS:\AppPools\$iisAppPoolName  
Set-ItemProperty IIS:\AppPools\$iisAppPoolName -name "managedRuntimeVersion" -value $iisAppPoolDotNetVersion  
}  
  
if (!(Test-Path IIS:\Sites\$iisWebsiteName -pathType container))  
{  
New-Item IIS:\Sites\$iisWebsiteName -bindings $iisWebsiteBindings -physicalPath $iisWebsiteFolderPath  
Set-ItemProperty IIS:\Sites\$iisWebsiteName -name applicationPool -value $iisAppPoolName  
}