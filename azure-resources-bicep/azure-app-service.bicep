
// -----------------------
// Variables (hardcoded)
// -----------------------
var location = 'West Europe'
var appName = 'doc-processor-api'
var planResourceId = '/subscriptions/50dd6081-f988-4fb9-b744-7f8019fa6806/resourceGroups/document-processor/providers/Microsoft.Web/serverfarms/api-free-plan'

var appHostname = '${appName}.azurewebsites.net'
var scmHostname = '${appName}.scm.azurewebsites.net'

var customDomainVerificationId = '303918AA883F342977051EE33EE5D2A07C82F8417662552F88F1E50877895D77'

// web config variables
var defaultDocuments = [
  'Default.htm'
  'Default.html'
  'Default.asp'
  'index.htm'
  'index.html'
  'iisstart.htm'
  'default.aspx'
  'index.php'
  'hostingstart.html'
]

var ipSecurityAllowAll = [
  {
    ipAddress: 'Any'
    action: 'Allow'
    priority: 2147483647
    name: 'Allow all'
    description: 'Allow all access'
  }
]

// deployment entries (names and timestamps kept as variables too)
var deployment1Name = 'c894c37ecbee43fda0df59c9fe70f10c'
var deployment1Start = '2025-12-29T21:35:59.9484489Z'
var deployment1End = '2025-12-29T21:36:01.7616525Z'

var deployment2Name = 'ebc3ce3634714aa5be9090ec4b2fd172'
var deployment2Start = '2025-12-29T21:45:47.525667Z'
var deployment2End = '2025-12-29T21:45:50.26006Z'

// -----------------------
// Resources
// -----------------------

resource site 'Microsoft.Web/sites@2024-11-01' = {
  name: appName
  location: location
  kind: 'app'
  properties: {
    enabled: true
    hostNameSslStates: [
      {
        name: appHostname
        sslState: 'Disabled'
        hostType: 'Standard'
      }
      {
        name: scmHostname
        sslState: 'Disabled'
        hostType: 'Repository'
      }
    ]
    serverFarmId: planResourceId
    reserved: false
    isXenon: false
    hyperV: false
    dnsConfiguration: {}
    outboundVnetRouting: {
      allTraffic: false
      applicationTraffic: false
      contentShareTraffic: false
      imagePullTraffic: false
      backupRestoreTraffic: false
    }
    siteConfig: {
      numberOfWorkers: 1
      acrUseManagedIdentityCreds: false
      alwaysOn: false
      http20Enabled: false
      functionAppScaleLimit: 0
      minimumElasticInstanceCount: 0
    }
    scmSiteAlsoStopped: false
    clientAffinityEnabled: true
    clientAffinityProxyEnabled: false
    clientCertEnabled: false
    clientCertMode: 'Required'
    hostNamesDisabled: false
    ipMode: 'IPv4'
    customDomainVerificationId: customDomainVerificationId
    containerSize: 0
    dailyMemoryTimeQuota: 0
    httpsOnly: true
    endToEndEncryptionEnabled: false
    redundancyMode: 'None'
    publicNetworkAccess: 'Enabled'
    storageAccountRequired: false
    keyVaultReferenceIdentity: 'SystemAssigned'
  }
}

resource siteFtpPolicy 'Microsoft.Web/sites/basicPublishingCredentialsPolicies@2024-11-01' = {
  parent: site
  name: 'ftp'
  location: location
  properties: {
    allow: true
  }
}

resource siteScmPolicy 'Microsoft.Web/sites/basicPublishingCredentialsPolicies@2024-11-01' = {
  parent: site
  name: 'scm'
  location: location
  properties: {
    allow: true
  }
}

resource siteWebConfig 'Microsoft.Web/sites/config@2024-11-01' = {
  parent: site
  name: 'web'
  location: location
  properties: {
    numberOfWorkers: 1
    defaultDocuments: defaultDocuments
    netFrameworkVersion: 'v8.0'
    requestTracingEnabled: false
    remoteDebuggingEnabled: false
    httpLoggingEnabled: false
    acrUseManagedIdentityCreds: false
    logsDirectorySizeLimit: 35
    detailedErrorLoggingEnabled: false
    publishingUsername: '$doc-processor-api'
    scmType: 'GitHubAction'
    use32BitWorkerProcess: true
    webSocketsEnabled: false
    alwaysOn: false
    managedPipelineMode: 'Integrated'
    virtualApplications: [
      {
        virtualPath: '/'
        physicalPath: 'site\\wwwroot'
        preloadEnabled: false
      }
    ]
    loadBalancing: 'LeastRequests'
    experiments: {
      rampUpRules: []
    }
    autoHealEnabled: false
    vnetRouteAllEnabled: false
    vnetPrivatePortsCount: 0
    publicNetworkAccess: 'Enabled'
    localMySqlEnabled: false
    ipSecurityRestrictions: ipSecurityAllowAll
    scmIpSecurityRestrictions: ipSecurityAllowAll
    scmIpSecurityRestrictionsUseMain: false
    http20Enabled: false
    minTlsVersion: '1.2'
    scmMinTlsVersion: '1.2'
