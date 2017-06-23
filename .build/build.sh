#!/bin/bash 
set -e

REPO_ROOT=`dirname "$0"`; REPO_ROOT=`eval "cd \"$REPO_ROOT/..\" && pwd"`

cd $REPO_ROOT

export Version=0.0.1

curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --install-dir .dotnet

dotnet restore

dotnet build

dotnet test test/Peaky.Client.Tests/Peaky.Client.Tests.csproj --logger "trx;LogFileName=${REPO_ROOT}/artifacts/TestResults/Peaky.Client.Tests.trx"

dotnet test test/Peaky.XUnit.Tests/Peaky.XUnit.Tests.csproj --logger "trx;LogFileName=${REPO_ROOT}/artifacts/TestResults/Peaky.XUnit.Tests.trx"

dotnet pack -o "$REPO_ROOT/artifacts/packages"

