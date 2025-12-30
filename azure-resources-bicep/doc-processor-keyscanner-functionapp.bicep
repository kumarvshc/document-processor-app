param sites_doc_processor_keyscanner_functionapp_name string = 'doc-processor-keyscanner-functionapp'
param serverfarms_ASP_documentprocessor_9dd3_externalid string = '/subscriptions/50dd6081-f988-4fb9-b744-7f8019fa6806/resourceGroups/document-processor/providers/Microsoft.Web/serverfarms/ASP-documentprocessor-9dd3'

resource sites_doc_processor_keyscanner_functionapp_name_resource 'Microsoft.Web/sites@2024-11-01' = {
  name: sites_doc_processor_keyscanner_functionapp_name
  location: 'West Europe'
  tags: {
    'hidden-link: /app-insights-resource-id': '/subscriptions/50dd6081-f988-4fb9-b744-7f8019fa6806/resourceGroups/document-processor/providers/microsoft.insights/components/doc-processor-keyscanner-functionapp'
  }
  kind: 'functionapp'
  properties: {
    enabled: true
    hostNameSslStates: [
      {
        name: '${sites_doc_processor_keyscanner_functionapp_name}.azurewebsites.net'
        sslState: 'Disabled'
        hostType: 'Standard'
      }
      {
        name: '${sites_doc_processor_keyscanner_functionapp_name}.scm.azurewebsites.net'
        sslState: 'Disabled'
        hostType: 'Repository'
      }
    ]
    serverFarmId: serverfarms_ASP_documentprocessor_9dd3_externalid
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
      functionAppScaleLimit: 200
      minimumElasticInstanceCount: 0
    }
    scmSiteAlsoStopped: false
    clientAffinityEnabled: false
    clientAffinityProxyEnabled: false
    clientCertEnabled: false
    clientCertMode: 'Required'
    hostNamesDisabled: false
    ipMode: 'IPv4'
    customDomainVerificationId: '303918AA883F342977051EE33EE5D2A07C82F8417662552F88F1E50877895D77'
    containerSize: 1536
    dailyMemoryTimeQuota: 0
    httpsOnly: true
    endToEndEncryptionEnabled: false
    redundancyMode: 'None'
    publicNetworkAccess: 'Enabled'
    storageAccountRequired: false
    keyVaultReferenceIdentity: 'SystemAssigned'
  }
}

resource sites_doc_processor_keyscanner_functionapp_name_ftp 'Microsoft.Web/sites/basicPublishingCredentialsPolicies@2024-11-01' = {
  parent: sites_doc_processor_keyscanner_functionapp_name_resource
  name: 'ftp'
  location: 'West Europe'
  tags: {
    'hidden-link: /app-insights-resource-id': '/subscriptions/50dd6081-f988-4fb9-b744-7f8019fa6806/resourceGroups/document-processor/providers/microsoft.insights/components/doc-processor-keyscanner-functionapp'
  }
  properties: {
    allow: false
  }
}

resource sites_doc_processor_keyscanner_functionapp_name_scm 'Microsoft.Web/sites/basicPublishingCredentialsPolicies@2024-11-01' = {
  parent: sites_doc_processor_keyscanner_functionapp_name_resource
  name: 'scm'
  location: 'West Europe'
  tags: {
    'hidden-link: /app-insights-resource-id': '/subscriptions/50dd6081-f988-4fb9-b744-7f8019fa6806/resourceGroups/document-processor/providers/microsoft.insights/components/doc-processor-keyscanner-functionapp'
  }
  properties: {
    allow: true
  }
}

resource sites_doc_processor_keyscanner_functionapp_name_web 'Microsoft.Web/sites/config@2024-11-01' = {
  parent: sites_doc_processor_keyscanner_functionapp_name_resource
  name: 'web'
  location: 'West Europe'
  tags: {
    'hidden-link: /app-insights-resource-id': '/subscriptions/50dd6081-f988-4fb9-b744-7f8019fa6806/resourceGroups/document-processor/providers/microsoft.insights/components/doc-processor-keyscanner-functionapp'
  }
  properties: {
    numberOfWorkers: 1
    defaultDocuments: [
      'Default.htm'
      'Default.html'
      'Default.asp'
      'index.htm'
      'index.html'
      'iisstart.htm'
      'default.aspx'
      'index.php'
    ]
    netFrameworkVersion: 'v8.0'
    requestTracingEnabled: false
    remoteDebuggingEnabled: false
    httpLoggingEnabled: false
    acrUseManagedIdentityCreds: false
    logsDirectorySizeLimit: 35
    detailedErrorLoggingEnabled: false
    publishingUsername: '$doc-processor-keyscanner-functionapp'
    scmType: 'GitHubAction'
    use32BitWorkerProcess: false
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
    cors: {
      allowedOrigins: [
        'https://portal.azure.com'
      ]
      supportCredentials: false
    }
    localMySqlEnabled: false
    ipSecurityRestrictions: [
      {
        ipAddress: 'Any'
        action: 'Allow'
        priority: 2147483647
        name: 'Allow all'
        description: 'Allow all access'
      }
    ]
    scmIpSecurityRestrictions: [
      {
        ipAddress: 'Any'
        action: 'Allow'
        priority: 2147483647
        name: 'Allow all'
        description: 'Allow all access'
      }
    ]
    scmIpSecurityRestrictionsUseMain: false
    http20Enabled: false
    minTlsVersion: '1.2'
    scmMinTlsVersion: '1.2'
    ftpsState: 'FtpsOnly'
    preWarmedInstanceCount: 0
    functionAppScaleLimit: 200
    functionsRuntimeScaleMonitoringEnabled: false
    minimumElasticInstanceCount: 0
    azureStorageAccounts: {}
    http20ProxyFlag: 0
  }
}

resource sites_doc_processor_keyscanner_functionapp_name_11456f17645e4cbdbb2cb63052e51538 'Microsoft.Web/sites/deployments@2024-11-01' = {
  parent: sites_doc_processor_keyscanner_functionapp_name_resource
  name: '11456f17645e4cbdbb2cb63052e51538'
  location: 'West Europe'
  properties: {
    status: 4
    author_email: 'N/A'
    author: 'N/A'
    deployer: 'GITHUB_ZIP_DEPLOY_FUNCTIONS_V1'
    message: '{"type":"deployment","sha":"895b09b8cb5eea00dce2519757f099809c7ea972","repoName":"kumarvshc/document-processor-app","actor":"kumarvshc","slotName":"production"}'
    start_time: '2025-12-30T00:09:32.5189176Z'
    end_time: '2025-12-30T00:09:34.6931657Z'
    active: true
  }
}

resource sites_doc_processor_keyscanner_functionapp_name_29bb9c4a47394cef8b04aba4f5b9b718 'Microsoft.Web/sites/deployments@2024-11-01' = {
  parent: sites_doc_processor_keyscanner_functionapp_name_resource
  name: '29bb9c4a47394cef8b04aba4f5b9b718'
  location: 'West Europe'
  properties: {
    status: 4
    author_email: 'N/A'
    author: 'N/A'
    deployer: 'GITHUB_ZIP_DEPLOY_FUNCTIONS_V1'
    message: '{"type":"deployment","sha":"ee67c24a1d49d9fe5ab241ba432afaf15f4f7a24","repoName":"kumarvshc/document-processor-app","actor":"kumarvshc","slotName":"production"}'
    start_time: '2025-12-30T00:03:27.0303231Z'
    end_time: '2025-12-30T00:03:28.8117853Z'
    active: false
  }
}

resource sites_doc_processor_keyscanner_functionapp_name_7713fd2ed4774309971404df6399d1fb 'Microsoft.Web/sites/deployments@2024-11-01' = {
  parent: sites_doc_processor_keyscanner_functionapp_name_resource
  name: '7713fd2ed4774309971404df6399d1fb'
  location: 'West Europe'
  properties: {
    status: 4
    author_email: 'N/A'
    author: 'N/A'
    deployer: 'GITHUB_ZIP_DEPLOY_FUNCTIONS_V1'
    message: '{"type":"deployment","sha":"af868799c5d2529e3f28544177c370c0631d3f33","repoName":"kumarvshc/document-processor-app","actor":"kumarvshc","slotName":"production"}'
    start_time: '2025-12-30T00:04:57.134501Z'
    end_time: '2025-12-30T00:04:59.0260711Z'
    active: false
  }
}

resource sites_doc_processor_keyscanner_functionapp_name_905476c86ae145c4ad47b267f8069823 'Microsoft.Web/sites/deployments@2024-11-01' = {
  parent: sites_doc_processor_keyscanner_functionapp_name_resource
  name: '905476c86ae145c4ad47b267f8069823'
  location: 'West Europe'
  properties: {
    status: 4
    author_email: 'N/A'
    author: 'N/A'
    deployer: 'GITHUB_ZIP_DEPLOY_FUNCTIONS_V1'
    message: '{"type":"deployment","sha":"6e80e3f218687885ac8e27835dd68db4f68e0525","repoName":"kumarvshc/document-processor-app","actor":"kumarvshc","slotName":"production"}'
    start_time: '2025-12-30T00:01:21.9408192Z'
    end_time: '2025-12-30T00:01:23.7064565Z'
    active: false
  }
}

resource sites_doc_processor_keyscanner_functionapp_name_cd2731c849ea424c92f03778a97ea543 'Microsoft.Web/sites/deployments@2024-11-01' = {
  parent: sites_doc_processor_keyscanner_functionapp_name_resource
  name: 'cd2731c849ea424c92f03778a97ea543'
  location: 'West Europe'
  properties: {
    status: 4
    author_email: 'N/A'
    author: 'N/A'
    deployer: 'GITHUB_ZIP_DEPLOY_FUNCTIONS_V1'
    message: '{"type":"deployment","sha":"b6e963facf1642e28adfd1daca9793ab0c0a7f21","repoName":"kumarvshc/document-processor-app","actor":"kumarvshc","slotName":"production"}'
    start_time: '2025-12-29T23:43:33.4552755Z'
    end_time: '2025-12-29T23:43:36.3640826Z'
    active: false
  }
}

resource sites_doc_processor_keyscanner_functionapp_name_fdb88c177dfd408e9f11a442b27cb024 'Microsoft.Web/sites/deployments@2024-11-01' = {
  parent: sites_doc_processor_keyscanner_functionapp_name_resource
  name: 'fdb88c177dfd408e9f11a442b27cb024'
  location: 'West Europe'
  properties: {
    status: 4
    author_email: 'N/A'
    author: 'N/A'
    deployer: 'GITHUB_ZIP_DEPLOY_FUNCTIONS_V1'
    message: '{"type":"deployment","sha":"158bc2f5bcbf480536db0fc612f34cda0cf74c77","repoName":"kumarvshc/document-processor-app","actor":"kumarvshc","slotName":"production"}'
    start_time: '2025-12-29T23:49:03.5133885Z'
    end_time: '2025-12-29T23:49:05.1704255Z'
    active: false
  }
}

resource sites_doc_processor_keyscanner_functionapp_name_KeyScanDocument 'Microsoft.Web/sites/functions@2024-11-01' = {
  parent: sites_doc_processor_keyscanner_functionapp_name_resource
  name: 'KeyScanDocument'
  location: 'West Europe'
  properties: {
    script_href: 'https://doc-processor-keyscanner-functionapp.azurewebsites.net/admin/vfs/site/wwwroot/DocumentProcessor.Functions.KeyScanner.dll'
    test_data_href: 'https://doc-processor-keyscanner-functionapp.azurewebsites.net/admin/vfs/data/Functions/sampledata/KeyScanDocument.dat'
    href: 'https://doc-processor-keyscanner-functionapp.azurewebsites.net/admin/functions/KeyScanDocument'
    config: {
      name: 'KeyScanDocument'
      entryPoint: 'DocumentProcessor.Functions.KeyScanner.KeyScannerFunction.ScanForDangerousContent'
      scriptFile: 'DocumentProcessor.Functions.KeyScanner.dll'
      language: 'dotnet-isolated'
      functionDirectory: ''
      bindings: [
        {
          name: 'message'
          type: 'serviceBusTrigger'
          direction: 'In'
          properties: {
            supportsDeferredBinding: 'True'
          }
          queueName: 'scan-documents'
          connection: 'ServiceBusConnection'
          cardinality: 'One'
        }
      ]
    }
    language: 'dotnet-isolated'
    isDisabled: false
  }
}

resource sites_doc_processor_keyscanner_functionapp_name_sites_doc_processor_keyscanner_functionapp_name_azurewebsites_net 'Microsoft.Web/sites/hostNameBindings@2024-11-01' = {
  parent: sites_doc_processor_keyscanner_functionapp_name_resource
  name: '${sites_doc_processor_keyscanner_functionapp_name}.azurewebsites.net'
  location: 'West Europe'
  properties: {
    siteName: 'doc-processor-keyscanner-functionapp'
    hostNameType: 'Verified'
  }
}
