
# clean up the previously-cached NuGet packages
Remove-Item -Recurse ~\.nuget\packages\peaky.xunit* -Force
Remove-Item -Recurse ~\.nuget\packages\peaky.client* -Force

# build and pack 
dotnet clean
dotnet build
dotnet pack /p:PackageVersion=5.0.0

# copy the packages to the temp directory
$destinationPath = "C:\temp\packages"
if (-not (Test-Path -Path $destinationPath -PathType Container)) {
    New-Item -Path $destinationPath -ItemType Directory -Force
}
Get-ChildItem -Recurse -Filter *.nupkg | Move-Item -Destination $destinationPath -Force

# delete the #r nuget caches
if (Test-Path -Path ~\.packagemanagement\nuget\Cache -PathType Container) {
    Remove-Item -Recurse -Force ~\.packagemanagement\nuget\Cache
}

if (Test-Path -Path ~\.packagemanagement\nuget\Projects -PathType Container) {
    Remove-Item -Recurse -Force ~\.packagemanagement\nuget\Projects
}
