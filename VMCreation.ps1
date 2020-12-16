$RGName = "todo"

$VMName = "todo"
$VMUserName = "todo"
$VMUserPassword = "todo"

$KVName = "todo"
$KVJwtSecret = "todo"

echo "Login"
az login

echo "Create resource group"
az group create --name $RGName --location westeurope

echo "Create vm"
az vm create --resource-group $RGName --name $VMName --image win2019datacenter --admin-username $VMUserName --admin-password $VMUserPassword

echo "Open ports"
az vm open-port --port 80 --resource-group $RGName --name $VMName --priority 100
az vm open-port --port 443 --resource-group $RGName --name $VMName --priority 110
az vm open-port --port 3389 --resource-group $RGName --name $VMName --priority 4000
az vm open-port --port 8172 --resource-group $RGName --name $VMName --priority 4010

echo "Create keyvault"
az keyvault create --name $KVName --resource-group $RGName

echo "Set secret"
az keyvault secret set --vault-name $KVName --name "JwtSecret" --value $KVJwtSecret

echo "Assign identity"
$VMIdentity = az vm identity assign --name $VMName --resource-group $RGName --query 'systemAssignedIdentity'

echo "Set keyvault policy"
az keyvault set-policy --name $KVName --object-id $VMIdentity --secret-permissions get list
