set -e
SCRIPTPATH="$( cd "$(dirname "$0")" ; pwd -P )"
source $SCRIPTPATH/env-vars.sh

cd ./Src/
func azure functionapp publish $nameFunction --dotnet-cli-params -- "--configuration Release"
cd -