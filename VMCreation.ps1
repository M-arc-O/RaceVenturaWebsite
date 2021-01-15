$AppName = "todo"
$Location = "westeurope"

$RGName = "$($AppName)RG"

$VMName = "$($AppName)VM"
$VMUserName = "todo"
$VMUserPassword = "todo"

$KVName = "$($AppName)KV"
$KVJwtSecret = "todo"

$SAName = "$($AppName.ToLower())sa"

echo "Login"
az login

echo "Create resource group"
az group create --name $RGName --location $Location

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

echo "Create storage account"
az storage account create --name $SAName --resource-group $RGName --location $Location --sku Standard_ZRS --encryption-services blob

echo "Enable static website"
az storage blob service-properties update --account-name $SAName --static-website --404-document 404.html --index-document index.html
