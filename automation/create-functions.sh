set -e

SCRIPTPATH="$( cd "$(dirname "$0")" ; pwd -P )"

source $SCRIPTPATH/env-vars.sh
az functionapp create --resource-group $nameResourceGroup \
  --consumption-plan-location $location \
  --name trgos-authorization \
  --storage-account $nameStorageAccount \
  --runtime dotnet