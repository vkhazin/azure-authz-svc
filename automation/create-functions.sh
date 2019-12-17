set -e

SCRIPTPATH="$( cd "$(dirname "$0")" ; pwd -P )"

source $SCRIPTPATH/env-vars.sh
az functionapp create --resource-group $nameResourceGroup \
  --consumption-plan-location $location \
  --name $nameFunction \
  --storage-account $nameStorageAccount \
  --runtime dotnet

az functionapp cors remove -g $nameResourceGroup -n $nameFunction --allowed-origins

az functionapp cors add --name $nameFunction \
--resource-group $nameResourceGroup \
--allowed-origins "*"
