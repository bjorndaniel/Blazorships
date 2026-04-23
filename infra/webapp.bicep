param location string = resourceGroup().location
param webAppName string
param appServicePlanName string

@description('SKU name. F1 = Free tier.')
param skuName string = 'F1'

@description('SKU tier. Must match skuName.')
param skuTier string = 'Free'

@description('Linux runtime stack for the Web App.')
param linuxFxVersion string = 'DOTNETCORE|10.0'

resource plan 'Microsoft.Web/serverfarms@2024-04-01' = {
  name: appServicePlanName
  location: location
  kind: 'linux'
  sku: {
    name: skuName
    tier: skuTier
  }
  properties: {
    reserved: true
  }
}

resource app 'Microsoft.Web/sites@2024-04-01' = {
  name: webAppName
  location: location
  kind: 'app,linux'
  properties: {
    serverFarmId: plan.id
    httpsOnly: true
    siteConfig: {
      linuxFxVersion: linuxFxVersion
      webSocketsEnabled: true
      ftpsState: 'Disabled'
      minTlsVersion: '1.2'
      http20Enabled: true
      alwaysOn: false
    }
  }
}

output webAppName string = app.name
output webAppUrl string = 'https://${app.properties.defaultHostName}'
