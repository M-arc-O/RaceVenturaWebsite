Write-Output "Import Module"
Import-Module AzureRM.KeyVault

Write-Output "Connect"
Connect-AzureRmAccount

$vaultName = "RaceVenturaKVC"
$certificateName = "raceventureacertificatebd9318c0-bf09-4f05-97a8-072a0cd5c245"
$pfxPath = [Environment]::GetFolderPath("Desktop") + "\$certificateName.pfx"
$password = "SomePassword"

Write-Output "Get secret"
$pfxSecret = Get-AzureKeyVaultSecret -VaultName $vaultName -Name $certificateName
$pfxUnprotectedBytes = [Convert]::FromBase64String($pfxSecret.SecretValueText)
$pfx = New-Object Security.Cryptography.X509Certificates.X509Certificate2
$pfx.Import($pfxUnprotectedBytes, $null, [Security.Cryptography.X509Certificates.X509KeyStorageFlags]::Exportable)
$pfxProtectedBytes = $pfx.Export([Security.Cryptography.X509Certificates.X509ContentType]::Pkcs12, $password)
[IO.File]::WriteAllBytes($pfxPath, $pfxProtectedBytes)