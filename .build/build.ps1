
$RepoRoot = Resolve-Path "$PSScriptRoot/../"

cd $RepoRoot

powershell -command "& ([scriptblock]::Create((Invoke-WebRequest -useb 'https://dot.net/v1/dotnet-install.ps1'))) -Architecture x64 -InstallDir .dotnet"

$env:PATH="$RepoRoot/.dotnet/;"+$env:PATH

dotnet restore

dotnet build

dotnet test test/Peaky.Client.Tests/Peaky.Client.Tests.csproj --logger "trx;LogFileName=${REPO_ROOT}/artifacts/TestResults/Peaky.Client.Tests.trx"

dotnet test test/Peaky.XUnit.Tests/Peaky.XUnit.Tests.csproj --logger "trx;LogFileName=${REPO_ROOT}/artifacts/TestResults/Peaky.XUnit.Tests.trx"

dotnet pack -o "$RepoRoot/artifacts/packages"