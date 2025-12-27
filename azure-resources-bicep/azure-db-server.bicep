@secure()
param vulnerabilityAssessments_Default_storageContainerPath string

param serverName string = 'document-processor'
param location string = 'northeurope'
param locationDisplayName string = 'North Europe'

// Admin / Identity
param sqlAdminLogin string = '' // your admin login
param aadAdminLogin string = '' // your add adming login (basically your email address)
param aadAdminSid string = '' // your aad admin Sid
param tenantId string = '' // your tenant Id

// SQL Server configuration
param sqlVersion string = '12.0'
param minimalTlsVersion string = '1.2'
param publicNetworkAccess string = 'Enabled'
param restrictOutboundNetworkAccess string = 'Disabled'
param azureADOnlyAuthentication bool = false

// Security defaults
param securityStateDisabled string = 'Disabled'
param auditingRetentionDays int = 0
param emailAccountAdmins bool = false

// Database configuration
param databaseName string = 'DocumentProcessor'
param databaseCollation string = 'SQL_Latin1_General_CP1_CI_AS'
param databaseMaxSizeBytes int = 34359738368
param zoneRedundant bool = false
param readScale string = 'Disabled'
param autoPauseDelay int = 60
param backupRedundancy string = 'Local'
param minCapacity number = 0.5
param maintenanceConfigurationId string = '/subscriptions/50dd6081-f988-4fb9-b744-7f8019fa6806/providers/Microsoft.Maintenance/publicMaintenanceConfigurations/SQL_Default'
param useFreeLimit bool = true
param freeLimitExhaustionBehavior string = 'AutoPause'
param availabilityZone string = 'NoPreference'

// SKU
param skuName string = 'GP_S_Gen5'
param skuTier string = 'GeneralPurpose'
param skuFamily string = 'Gen5'
param skuCapacity int = 2

// Firewall
param allowAzureIpsStart string = '0.0.0.0'
param allowAzureIpsEnd string = '0.0.0.0'
param allowAllIpsStart string = '0.0.0.0'
param allowAllIpsEnd string = '255.255.255.255'

// Advisor auto-execute
param advisorEnabled string = 'Enabled'
param advisorDisabled string = 'Disabled'

// Subscription placeholders
param emptySubscriptionId string = '00000000-0000-0000-0000-000000000000'

resource sqlServer 'Microsoft.Sql/servers@2024-05-01-preview' = {
  name: serverName
  location: location
  kind: 'v12.0'
  properties: {
    administratorLogin: sqlAdminLogin
    version: sqlVersion
    minimalTlsVersion: minimalTlsVersion
    publicNetworkAccess: publicNetworkAccess
    restrictOutboundNetworkAccess: restrictOutboundNetworkAccess

    administrators: {
      administratorType: 'ActiveDirectory'
      principalType: 'User'
      login: aadAdminLogin
      sid: aadAdminSid
      tenantId: tenantId
      azureADOnlyAuthentication: azureADOnlyAuthentication
    }
  }
}

resource documentProcessorDb 'Microsoft.Sql/servers/databases@2024-05-01-preview' = {
  parent: sqlServer
  name: databaseName
  location: location
  sku: {
    name: skuName
    tier: skuTier
    family: skuFamily
    capacity: skuCapacity
  }
  kind: 'v12.0,user,vcore,serverless,freelimit'
  properties: {
    collation: databaseCollation
    maxSizeBytes: databaseMaxSizeBytes
    catalogCollation: databaseCollation
    zoneRedundant: zoneRedundant
    readScale: readScale
    autoPauseDelay: autoPauseDelay
    requestedBackupStorageRedundancy: backupRedundancy
    minCapacity: minCapacity
    maintenanceConfigurationId: maintenanceConfigurationId
    useFreeLimit: useFreeLimit
    freeLimitExhaustionBehavior: freeLimitExhaustionBehavior
    availabilityZone: availabilityZone
    isLedgerOn: false
  }
}

resource allowAzureIps 'Microsoft.Sql/servers/firewallRules@2024-05-01-preview' = {
  parent: sqlServer
  name: 'AllowAllWindowsAzureIps'
  properties: {
    startIpAddress: allowAzureIpsStart
    endIpAddress: allowAzureIpsEnd
  }
}

resource allowAllIps 'Microsoft.Sql/servers/firewallRules@2024-05-01-preview' = {
  parent: sqlServer
  name: 'AllowAllIps'
  properties: {
    startIpAddress: allowAllIpsStart
    endIpAddress: allowAllIpsEnd
  }
}

resource forceLastGoodPlan 'Microsoft.Sql/servers/advisors@2014-04-01' = {
  parent: sqlServer
  name: 'ForceLastGoodPlan'
  properties: {
    autoExecuteValue: advisorEnabled
  }
}

