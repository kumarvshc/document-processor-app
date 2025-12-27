
// =======================
// Parameters
// =======================

@description('Service Bus namespace name')
param namespaces_document_processor_name string = 'document-processor'

// If you want to override queue list per environment, keep it as a param.
// You can also turn this into a var if it never changes.
@description('Service Bus queue names to create')
param queueNames array = [
  'pattern-documents'
  'scan-documents'
]

// =======================
// Variables (all values centralized)
// =======================

@description('Azure region to deploy resources into')
var location = 'northeurope'

// Namespace config (example of centralizing config in vars)
var namespaceSkuName = 'Basic'
var namespaceSkuTier = 'Basic'
var namespacePremiumMessagingPartitions = 0
var namespaceMinimumTlsVersion = '1.2'
var namespacePublicNetworkAccess = 'Enabled'
var namespaceDisableLocalAuth = false
var namespaceZoneRedundant = true

// Network ruleset defaults (if you deploy it)
var networkRulesetPublicNetworkAccess = 'Enabled'
var networkRulesetDefaultAction = 'Allow'
var networkRulesetVirtualNetworkRules = []
var networkRulesetIpRules = []
var networkRulesetTrustedServiceAccessEnabled = false

// Queue properties centralized as one object
var queueProps = {
  maxMessageSizeInKilobytes: 256
  lockDuration: 'PT1M'
  maxSizeInMegabytes: 1024
  requiresDuplicateDetection: false
  requiresSession: false
  defaultMessageTimeToLive: 'P14D'
  deadLetteringOnMessageExpiration: false
  enableBatchedOperations: true
  duplicateDetectionHistoryTimeWindow: 'PT10M'
  maxDeliveryCount: 10
  status: 'Active'
  // Azure’s “max” timespan constant retained from your original
  autoDeleteOnIdle: 'P10675199DT2H48M5.4775807S'
  enablePartitioning: false
  enableExpress: false
}

// =======================
// Resources
// =======================

resource sbNamespace 'Microsoft.ServiceBus/namespaces@2024-01-01' = {
  name: namespaces_document_processor_name
  location: location
  sku: {
    name: namespaceSkuName
    tier: namespaceSkuTier
  }
  properties: {
    premiumMessagingPartitions: namespacePremiumMessagingPartitions
    minimumTlsVersion: namespaceMinimumTlsVersion
    publicNetworkAccess: namespacePublicNetworkAccess
    disableLocalAuth: namespaceDisableLocalAuth
    zoneRedundant: namespaceZoneRedundant
  }
}

// Optional: RootManageSharedAccessKey (kept for completeness)
resource sbNamespaceRootKey 'Microsoft.ServiceBus/namespaces/authorizationrules@2024-01-01' = {
  parent: sbNamespace
  name: 'RootManageSharedAccessKey'
  location: location
  properties: {
    rights: [
      'Listen'
      'Manage'
      'Send'
    ]
  }
}

// Optional: Network ruleset (if you need it)
resource sbNamespaceNetworkRules 'Microsoft.ServiceBus/namespaces/networkrulesets@2024-01-01' = {
  parent: sbNamespace
  name: 'default'
  location: location
  properties: {
    publicNetworkAccess: networkRulesetPublicNetworkAccess
    defaultAction: networkRulesetDefaultAction
    virtualNetworkRules: networkRulesetVirtualNetworkRules
    ipRules: networkRulesetIpRules
    trustedServiceAccessEnabled: networkRulesetTrustedServiceAccessEnabled
  }
}

// Queues: create one per name in queueNames, reusing the centralized queueProps
resource sbQueues 'Microsoft.ServiceBus/namespaces/queues@2024-01-01' = [for q in queueNames: {
  parent: sbNamespace
  name: q
  location: location
  properties: queueProps
}]
