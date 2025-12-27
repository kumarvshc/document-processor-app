
// ==============================
// Parameters
// ==============================

// Server name segment (parent server must already exist in the same template/scope or via existing reference)
@description('The SQL Server name that hosts the database.')
param sqlServerName string

@description('Database logical name.')
param databaseName string = 'DocumentProcessor'

@description('Deployment location (Azure region).')
@allowed([
  'northeurope'
  'westeurope'
  'westus'
  'eastus'
  'uksouth'
  'centralus'
])
param location string = 'northeurope'

// ---------- SKU & compute ----------
@description('SKU name for Azure SQL DB.')
param skuName string = 'GP_S_Gen5'

@description('SKU tier.')
@allowed([
  'GeneralPurpose'
  'BusinessCritical'
  'Hyperscale'
])
param skuTier string = 'GeneralPurpose'

@description('Compute family.')
@allowed([
  'Gen5'
  'StandardSeries'
  'PremiumSeries'
])
param skuFamily string = 'Gen5'

@description('vCore capacity for the DB (for serverless, this is the max vCores).')
@minValue(1)
param skuCapacity int = 2

// ---------- DB kind ----------
@description('Database kind flags.')
param dbKind string = 'v12.0,user,vcore,serverless,freelimit'

// ---------- Collations ----------
@description('Database collation.')
param dbCollation string = 'SQL_Latin1_General_CP1_CI_AS'

@description('Catalog collation.')
param catalogCollation string = 'SQL_Latin1_General_CP1_CI_AS'

// ---------- Size & HA ----------
@description('Max database size in bytes (default 32 GB).')
@minValue(2147483648) // 2 GB
param maxSizeBytes int = 34359738368

@description('Zone redundant setting.')
param zoneRedundant bool = false

@description('Read scale setting.')
@allowed([
  'Enabled'
  'Disabled'
])
param readScale string = 'Disabled'

// ---------- Serverless controls ----------
@description('Auto-pause delay in minutes for serverless (0 disables autopause).')
@minValue(0)
param autoPauseDelay int = 60

@description('Minimum vCores for serverless.')
@minValue(0.5)
param minCapacity number = 0.5

@description('Use free limit for serverless.')
param useFreeLimit bool = true

@description('Behavior when free limit is exhausted.')
@allowed([
  'AutoPause'
  'PayAsYouGo'
])
param freeLimitExhaustionBehavior string = 'AutoPause'

// ---------- Backup redundancy ----------
@description('Requested backup storage redundancy.')
@allowed([
  'Local'
  'Zone'
  'Geo'
])
param requestedBackupStorageRedundancy string = 'Local'

// ---------- Maintenance ----------
@description('Maintenance configuration resource ID.')
param maintenanceConfigurationId string = '/subscriptions/50dd6081-f988-4fb9-b744-7f8019fa6806/providers/Microsoft.Maintenance/publicMaintenanceConfigurations/SQL_Default'

// ---------- Availability Zone preference ----------
@description('Availability zone preference.')
@allowed([
  'NoPreference'
  '1'
  '2'
  '3'
])
param availabilityZone string = 'NoPreference'

// ---------- Ledger ----------
@description('Enable ledger.')
param isLedgerOn bool = false

// ---------- Child resource names (override only if needed) ----------
param atpSettingName string = 'Default'
param geoBackupPolicyName string = 'Default'
param tdeName string = 'Current'
param ledgerDigestUploadName string = 'Current'
param auditingClassicName string = 'Default'
param auditingExtendedName string = 'default'
param auditingSettingsName string = 'default'
param ltrPolicyName string = 'default'
param strPolicyName string = 'default'

// ---------- Auditing (classic) ----------
@description('Classic auditing state.')
@allowed([
  'Enabled'
  'Disabled'
])
param classicAuditingState string = 'Disabled'

// Classic auditing historically used a location string; keeping configurable
@description('Classic auditing location label (not a region code).')
param classicAuditingLocation string = 'North Europe'

// ---------- Auditing (extended) ----------
@description('Extended auditing state.')
@allowed([
  'Enabled'
  'Disabled'
])
param extendedAuditingState string = 'Disabled'

@description('Extended auditing retention (days). 0 = unlimited.')
@minValue(0)
param extendedAuditingRetentionDays int = 0

@description('Enable Azure Monitor sink for extended auditing.')
param isAzureMonitorTargetEnabled bool = false

@description('Subscription ID of the storage account receiving audit logs (0000 GUID disables).')
param storageAccountSubscriptionId string = '00000000-0000-0000-0000-000000000000'

// ---------- Security alert policy ----------
@description('Security alert policy state.')
@allowed([
  'Enabled'
  'Disabled'
])
param securityAlertPolicyState string = 'Disabled'

@description('Disabled alert types (e.g., "Sql_Injection", "Data_Exfiltration").')
param disabledAlerts array = [
  ''
]

@description('Email recipients for security alerts.')
param emailAddresses array = [
  ''
]

@description('Send alerts to subscription admins.')
param emailAccountAdmins bool = false

@description('Retention in days for security alerts (0 = unlimited).')
@minValue(0)
param securityAlertRetentionDays int = 0

// ---------- Backup retention ----------
@description('Short-term retention days (7-35 typical).')
@minValue(1)
param shortTermRetentionDays int = 7

@description('Differential backup interval in hours.')
@minValue(0)
param diffBackupIntervalInHours int = 12

@description('Weekly LTR retention duration ISO8601 (e.g. "PT0S" to disable, "P1W", "P4W").')
param ltrWeeklyRetention string = 'PT0S'

@description('Monthly LTR retention duration ISO8601 (e.g. "PT0S", "P1M", "P3M").')
param ltrMonthlyRetention string = 'PT0S'

@description('Yearly LTR retention duration ISO8601 (e.g. "PT0S", "P1Y", "P5Y").')
param ltrYearlyRetention string = 'PT0S'

@description('Week of year for yearly backups (1-52; 0 means ignore).')
@minValue(0)
@maxValue(52)
param ltrWeekOfYear int = 0

// ---------- Vulnerability assessment ----------
@description('Enable recurring vulnerability scans.')
param vaRecurringScansEnabled bool = false

@description('Email subscription admins on vulnerability scans.')
param vaEmailSubscriptionAdmins bool = true

// ---------- Advisors auto-execute flags ----------
@description('Advisor: CreateIndex autoExecuteValue.')
@allowed(['Enabled', 'Disabled'])
param advisorCreateIndex string = 'Disabled'

@description('Advisor: DbParameterization autoExecuteValue.')
@allowed(['Enabled', 'Disabled'])
param advisorDbParameterization string = 'Disabled'

@description('Advisor: DefragmentIndex autoExecuteValue.')
@allowed(['Enabled', 'Disabled'])
param advisorDefragmentIndex string = 'Disabled'

@description('Advisor: DropIndex autoExecuteValue.')
@allowed(['Enabled', 'Disabled'])
param advisorDropIndex string = 'Disabled'

@description('Advisor: ForceLastGoodPlan autoExecuteValue.')
@allowed(['Enabled', 'Disabled'])
param advisorForceLastGoodPlan string = 'Enabled'


// ==============================
// Variables
// ==============================

var dbResourceName = '${sqlServerName}/${databaseName}'


// ==============================
// Resources
// ==============================

resource db 'Microsoft.Sql/servers/databases@2024-05-01-preview' = {
  name: dbResourceName
  location: location
  sku: {
    name: skuName
    tier: skuTier
    family: skuFamily
    capacity: skuCapacity
  }
  kind: dbKind
  properties: {
    collation: dbCollation
    maxSizeBytes: maxSizeBytes
    catalogCollation: catalogCollation
    zoneRedundant: zoneRedundant
    readScale: readScale
    autoPauseDelay: autoPauseDelay
    requestedBackupStorageRedundancy: requestedBackupStorageRedundancy
    minCapacity: minCapacity
    maintenanceConfigurationId: maintenanceConfigurationId
    isLedgerOn: isLedgerOn
    useFreeLimit: useFreeLimit
    freeLimitExhaustionBehavior: freeLimitExhaustionBehavior
    availabilityZone: availabilityZone
  }
}

resource atp 'Microsoft.Sql/servers/databases/advancedThreatProtectionSettings@2024-05-01-preview' = {
  parent: db
  name: atpSettingName
  properties: {
    state: 'Disabled'
  }
}

resource advisorCreateIndex 'Microsoft.Sql/servers/databases/advisors@2014-04-01' = {
  parent: db
  name: 'CreateIndex'
  properties: {
    autoExecuteValue: advisorCreateIndex
  }
}

resource advisorDbParameterization 'Microsoft.Sql/servers/databases/advisors@2014-04-01' = {
  parent: db
  name: 'DbParameterization'
  properties: {
    autoExecuteValue: advisorDbParameterization
  }
}

resource advisorDefragmentIndex 'Microsoft.Sql/servers/databases/advisors@2014-04-01' = {
  parent: db
  name: 'DefragmentIndex'
  properties: {
    autoExecuteValue: advisorDefragmentIndex
  }
}

resource advisorDropIndex 'Microsoft.Sql/servers/databases/advisors@2014-04-01' = {
  parent: db
  name: 'DropIndex'
  properties: {
    autoExecuteValue: advisorDropIndex
  }
}

resource advisorForceLastGoodPlan 'Microsoft.Sql/servers/databases/advisors@2014-04-01' = {
  parent: db
  name: 'ForceLastGoodPlan'
  properties: {
    autoExecuteValue: advisorForceLastGoodPlan
  }
}

resource auditingClassic 'Microsoft.Sql/servers/databases/auditingPolicies@2014-04-01' = {
  parent: db
  name: auditingClassicName
  location: classicAuditingLocation
  properties: {
    auditingState: classicAuditingState
  }
}

resource auditingSettings 'Microsoft.Sql/servers/databases/auditingSettings@2024-05-01-preview' = {
  parent: db
  name: auditingSettingsName
  properties: {
    retentionDays: extendedAuditingRetentionDays
    isAzureMonitorTargetEnabled: isAzureMonitorTargetEnabled
    state: extendedAuditingState
    storageAccountSubscriptionId: storageAccountSubscriptionId
  }
}

resource ltrPolicy 'Microsoft.Sql/servers/databases/backupLongTermRetentionPolicies@2024-05-01-preview' = {
  parent: db
  name: ltrPolicyName
  properties: {
    weeklyRetention: ltrWeeklyRetention
    monthlyRetention: ltrMonthlyRetention
    yearlyRetention: ltrYearlyRetention
    weekOfYear: ltrWeekOfYear
  }
}

resource strPolicy 'Microsoft.Sql/servers/databases/backupShortTermRetentionPolicies@2024-05-01-preview' = {
  parent: db
  name: strPolicyName
  properties: {
    retentionDays: shortTermRetentionDays
    diffBackupIntervalInHours: diffBackupIntervalInHours
  }
}

resource extendedAuditing 'Microsoft.Sql/servers/databases/extendedAuditingSettings@2024-05-01-preview' = {
  parent: db
  name: auditingExtendedName
  properties: {
    retentionDays: extendedAuditingRetentionDays
    isAzureMonitorTargetEnabled: isAzureMonitorTargetEnabled
    state: extendedAuditingState
    storageAccountSubscriptionId: storageAccountSubscriptionId
  }
}

resource geoBackupPolicy 'Microsoft.Sql/servers/databases/geoBackupPolicies@2024-05-01-preview' = {
  parent: db
  name: geoBackupPolicyName
  properties: {
    state: 'Disabled'
  }
}

resource ledgerDigestUploads 'Microsoft.Sql/servers/databases/ledgerDigestUploads@2024-05-01-preview' = {
  parent: db
  name: ledgerDigestUploadName
  properties: {}
}

resource securityAlertPolicies 'Microsoft.Sql/servers/databases/securityAlertPolicies@2024-05-01-preview' = {
  parent: db
  name: 'Default'
  properties: {
    state: securityAlertPolicyState
    disabledAlerts: disabledAlerts
    emailAddresses: emailAddresses
    emailAccountAdmins: emailAccountAdmins
    retentionDays: securityAlertRetentionDays
  }
}

resource tde 'Microsoft.Sql/servers/databases/transparentDataEncryption@2024-05-01-preview' = {
  parent: db
  name: tdeName
  properties: {
    state: 'Enabled'
  }
}

resource vulnerabilityAssessments 'Microsoft.Sql/servers/databases/vulnerabilityAssessments@2024-05-01-preview' = {
  parent: db
  name: 'Default'
  properties: {
    recurringScans: {
      isEnabled: vaRecurringScansEnabled
      emailSubscriptionAdmins: vaEmailSubscriptionAdmins
    }
  }
}
