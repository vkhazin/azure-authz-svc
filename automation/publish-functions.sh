set -e
SCRIPTPATH="$( cd "$(dirname "$0")" ; pwd -P )"
source $SCRIPTPATH/env-vars.sh

cd ./src/
func azure functionapp publish trgos-authorization --dotnet-cli-params -- "--configuration Release"
cd -