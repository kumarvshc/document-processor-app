param vaults_doc_processor_keyvault_name string = 'doc-processor-keyvault'

resource vaults_doc_processor_keyvault_name_resource 'Microsoft.KeyVault/vaults@2024-12-01-preview' = {
  name: vaults_doc_processor_keyvault_name
  location: 'northeurope'
  properties: {
    sku: {
      family: 'A'
      name: 'Standard'
    }
    tenantId: '23975456-3418-419e-ad53-33ff4161387e'
    networkAcls: {
      bypass: 'None'
      defaultAction: 'Allow'
      ipRules: []
      virtualNetworkRules: []
    }
    accessPolicies: []
    enabledForDeployment: false
    enabledForDiskEncryption: false
    enabledForTemplateDeployment: false
    enableSoftDelete: true
    softDeleteRetentionInDays: 90
    enableRbacAuthorization: true
    vaultUri: 'https://${vaults_doc_processor_keyvault_name}.vault.azure.net/'
    provisioningState: 'Succeeded'
    publicNetworkAccess: 'Enabled'
  }
}

resource vaults_doc_processor_keyvault_name_ServiceBusConnection 'Microsoft.KeyVault/vaults/secrets@2024-12-01-preview' = {
  parent: vaults_doc_processor_keyvault_name_resource
  name: 'ServiceBusConnection'
  location: 'northeurope'
  properties: {
    attributes: {
      enabled: true
    }
  }
}

resource vaults_doc_processor_keyvault_name_SqlConnection 'Microsoft.KeyVault/vaults/secrets@2024-12-01-preview' = {
  parent: vaults_doc_processor_keyvault_name_resource
  name: 'SqlConnection'
  location: 'northeurope'
  properties: {
    attributes: {
      enabled: true
    }
  }
}
