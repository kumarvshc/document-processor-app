@description('Azure region')
param location string = 'West Europe'

@description('Subscription ID')
param subscriptionId string = '50dd6081-f988-4fb9-b744-7f8019fa6806'

@description('Resource group name')
param resourceGroupName string = 'document-processor'

@description('Function App name')
param functionAppName string = 'doc-processor-extractpattern-functionapp'

@description('App Service Plan resource ID')
param appServicePlanId string = '/subscriptions/50dd6081-f988-4fb9-b744-7f8019fa6806/resourceGroups/document-processor/providers/Microsoft.Web/serverfarms/ASP-documentprocessor-91ea'

@description('Application Insights name')
param appInsightsName string = 'doc-processor-extractpattern-functionapp'

@description('Function name')
param functionName string = 'ExtractPatterns'

@description('Service Bus queue name')
param serviceBusQueueName string = 'pattern-documents'

@description('Service Bus connection setting name')
param serviceBusConnectionSetting string = 'ServiceBusConnection'

@description('Custom domain verification id')
param customDomainVerificationId string = '303918AA883F342977051EE33EE5D2A07C82F8417662552F88F1E50877895D77'

/* ===========================
   Variables
   =========================== */

var appInsightsResourceId = '/subscriptions/${subscriptionId}/resourceGroups/${resourceGroupName}/providers/microsoft.insights/components/${appInsightsName}'
var defaultHostname = '${functionAppName}.azurewebsites.net'
var scmHostname = '${functionAppName}.scm.azurewebsites.net'
var publishingUsername = '$${functionAppName}'
var dotnetVersion = 'v8.0'
var runtimeLanguage = 'dotnet-isolated'

var allowAllIpRule = {
  ipAddress: 'Any'
  action: 'Allow'
  priority: 2147483647
  name: 'Allow all'
  description: 'Allow all access'
}

/* ===========================
   Function App
   =========================== */

resource functionApp 'Microsoft.Web/sites@2024-11-01' = {
  name: functionAppName
  location: location
  kind: 'functionapp'
  tags: {
    'hidden-link: /app-insights-resource-id': appInsightsResourceId
  }
  properties: {
    enabled: true
    serverFarmId: appServicePlanId
    reserved: false
    hyperV: false
    isXenon: false
    httpsOnly: true
    publicNetworkAccess: 'Enabled'
    storageAccountRequired: false
    keyVaultReferenceIdentity: 'SystemAssigned'
    containerSize: 1536
    dailyMemoryTimeQuota: 0
    redundancyMode: 'None'
    ipMode: 'IPv4'
    customDomainVerificationId: customDomainVerificationId

    hostNameSslStates: [
      {
        name: defaultHostname
        sslState: 'Disabled'
        hostType: 'Standard'
      }
      {
        name: scmHostname
        sslState: 'Disabled'
        hostType: 'Repository'
      }
    ]

    outboundVnetRouting: {
      allTraffic: false
      applicationTraffic: false
      contentShareTraffic: false
      imagePullTraffic: false
      backupRestoreTraffic: false
    }

    siteConfig: {
      numberOfWorkers: 1
      alwaysOn: false
      http20Enabled: false
      functionAppScaleLimit: 200
      minimumElasticInstanceCount: 0
      acrUseManagedIdentityCreds: false
    }
  }
}

/* ===========================
   Publishing Credential Policies
   =========================== */

resource ftpPolicy 'Microsoft.Web/sites/basicPublishingCredentialsPolicies@2024-11-01' = {
  parent: functionApp
  name: 'ftp'
  location: location
  tags: {
    'hidden-link: /app-insights-resource-id': appInsightsResourceId
  }
  properties: {
    allow: false
  }
}

resource scmPolicy 'Microsoft.Web/sites/basicPublishingCredentialsPolicies@2024-11-01' = {
  parent: functionApp
  name: 'scm'
  location: location
  tags: {
    'hidden-link: /app-insights-resource-id': appInsightsResourceId
  }
  properties: {
    allow: true
  }
}

/* ===========================
   Web Config
   =========================== */

resource webConfig 'Microsoft.Web/sites/config@2024-11-01' = {
  parent: functionApp
  name: 'web'
  location: location
  tags: {
    'hidden-link: /app-insights-resource-id': appInsightsResourceId
  }
  properties: {
    numberOfWorkers: 1
    netFrameworkVersion: dotnetVersion
    publishingUsername: publishingUsername
    scmType: 'None'
    use32BitWorkerProcess: false
    alwaysOn: false
    managedPipelineMode: 'Integrated'
    webSocketsEnabled: false
    http20Enabled: false
    minTlsVersion: '1.2'
    scmMinTlsVersion: '1.2'
    ftpsState: 'FtpsOnly'
    functionAppScaleLimit: 200
    minimumElasticInstanceCount: 0
    publicNetworkAccess: 'Enabled'
    logsDirectorySizeLimit: 35

    cors: {
      allowedOrigins: [
        'https://portal.azure.com'
      ]
      supportCredentials: false
    }

    ipSecurityRestrictions: [
      allowAllIpRule
    ]

    scmIpSecurityRestrictions: [
      allowAllIpRule
    ]

    scmIpSecurityRestrictionsUseMain: false
    azureStorageAccounts: {}
  }
}

/* ===========================
   Function Definition
   =========================== */

resource functionResource 'Microsoft.Web/sites/functions@2024-11-01' = {
  parent: functionApp
  name: functionName
  location: location
  properties: {
    language: runtimeLanguage
    isDisabled: false
    config: {
      name: functionName
      entryPoint: 'DocumentProcessor.Functions.ExtractPattern.ExtractPatternFunction.ExtractPatterns'
      scriptFile: 'DocumentProcessor.Functions.ExtractPattern.dll'
      language: runtimeLanguage
      bindings: [
        {
          name: 'message'
          type: 'serviceBusTrigger'
          direction: 'In'
          queueName: serviceBusQueueName
          connection: serviceBusConnectionSetting
          cardinality: 'One'
          properties: {
            supportsDeferredBinding: 'True'
          }
        }
      ]
    }
  }
}

/* ===========================
   Hostname Binding
   =========================== */

resource hostnameBinding 'Microsoft.Web/sites/hostNameBindings@2024-11-01' = {
  parent: functionApp
  name: defaultHostname
  location: location
  properties: {
    siteName: functionAppName
    hostNameType: 'Verified'
  }
}
