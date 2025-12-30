@description('Function App name')
param functionAppName string = 'doc-processor-keyscanner-functionapp'

@description('Deployment location')
param location string = 'West Europe'

@description('App Service Plan resource ID')
param appServicePlanId string = '/subscriptions/50dd6081-f988-4fb9-b744-7f8019fa6806/resourceGroups/document-processor/providers/Microsoft.Web/serverfarms/ASP-documentprocessor-9dd3'

@description('Application Insights resource ID')
param appInsightsResourceId string = '/subscriptions/50dd6081-f988-4fb9-b744-7f8019fa6806/resourceGroups/document-processor/providers/microsoft.insights/components/doc-processor-keyscanner-functionapp'

@description('Custom domain verification ID')
param customDomainVerificationId string = '303918AA883F342977051EE33EE5D2A07C82F8417662552F88F1E50877895D77'

@description('Service Bus queue name')
param serviceBusQueueName string = 'scan-documents'

@description('Service Bus connection setting name')
param serviceBusConnectionSetting string = 'ServiceBusConnection'

@description('GitHub repository name')
param gitHubRepoName string = '{your repo name}/document-processor-app'

@description('GitHub actor')
param gitHubActor string = '' /*your actor name*/


/* ----------------------------- VARIABLES ----------------------------- */

var azureWebsitesDomain = 'azurewebsites.net'
var scmSubdomain = 'scm'
var httpsScheme = 'https'

var functionKind = 'functionapp'
var dotnetIsolated = 'dotnet-isolated'
var tlsVersion = '1.2'

var publishingUsername = '$${functionAppName}'


/* ----------------------------- FUNCTION APP ----------------------------- */

resource functionApp 'Microsoft.Web/sites@2024-11-01' = {
  name: functionAppName
  location: location
  kind: functionKind
  tags: {
    'hidden-link: /app-insights-resource-id': appInsightsResourceId
  }
  properties: {
    enabled: true
    serverFarmId: appServicePlanId
    httpsOnly: true
    publicNetworkAccess: 'Enabled'
    storageAccountRequired: false
    keyVaultReferenceIdentity: 'SystemAssigned'
    customDomainVerificationId: customDomainVerificationId

    hostNameSslStates: [
      {
        name: '${functionAppName}.${azureWebsitesDomain}'
        sslState: 'Disabled'
        hostType: 'Standard'
      }
      {
        name: '${functionAppName}.${scmSubdomain}.${azureWebsitesDomain}'
        sslState: 'Disabled'
        hostType: 'Repository'
      }
    ]

    siteConfig: {
      numberOfWorkers: 1
      alwaysOn: false
      functionAppScaleLimit: 200
      minimumElasticInstanceCount: 0
    }
  }
}


/* ----------------------------- FTP POLICY ----------------------------- */

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


/* ----------------------------- SCM POLICY ----------------------------- */

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


/* ----------------------------- WEB CONFIG ----------------------------- */

resource webConfig 'Microsoft.Web/sites/config@2024-11-01' = {
  parent: functionApp
  name: 'web'
  location: location
  tags: {
    'hidden-link: /app-insights-resource-id': appInsightsResourceId
  }
  properties: {
    netFrameworkVersion: 'v8.0'
    scmType: 'GitHubAction'
    publishingUsername: publishingUsername
    minTlsVersion: tlsVersion
    scmMinTlsVersion: tlsVersion
    ftpsState: 'FtpsOnly'
    functionAppScaleLimit: 200

    cors: {
      allowedOrigins: [
        '${httpsScheme}://portal.azure.com'
      ]
      supportCredentials: false
    }

    ipSecurityRestrictions: [
      {
        ipAddress: 'Any'
        action: 'Allow'
        priority: 2147483647
        name: 'Allow all'
      }
    ]

    scmIpSecurityRestrictions: [
      {
        ipAddress: 'Any'
        action: 'Allow'
        priority: 2147483647
        name: 'Allow all'
      }
    ]
  }
}


/* ----------------------------- FUNCTION DEFINITION ----------------------------- */

resource keyScanFunction 'Microsoft.Web/sites/functions@2024-11-01' = {
  parent: functionApp
  name: 'KeyScanDocument'
  location: location
  properties: {
    language: dotnetIsolated
    isDisabled: false
    config: {
      name: 'KeyScanDocument'
      entryPoint: 'DocumentProcessor.Functions.KeyScanner.KeyScannerFunction.ScanForDangerousContent'
      scriptFile: 'DocumentProcessor.Functions.KeyScanner.dll'
      language: dotnetIsolated
      bindings: [
        {
          name: 'message'
          type: 'serviceBusTrigger'
          direction: 'In'
          queueName: serviceBusQueueName
          connection: serviceBusConnectionSetting
          cardinality: 'One'
        }
      ]
    }
  }
}


/* ----------------------------- HOSTNAME BINDING ----------------------------- */

resource hostnameBinding 'Microsoft.Web/sites/hostNameBindings@2024-11-01' = {
  parent: functionApp
  name: '${functionAppName}.${azureWebsitesDomain}'
  location: location
  properties: {
    siteName: functionAppName
    hostNameType: 'Verified'
  }
}
