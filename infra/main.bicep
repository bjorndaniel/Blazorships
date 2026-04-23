targetScope = 'subscription'

@description('Azure region for the resource group and web app.')
param location string = 'westeurope'

@description('Name of the resource group to create or update.')
param resourceGroupName string = 'blazorships-rg'

@description('Globally unique name of the Web App.')
param webAppName string

@description('Name of the App Service Plan.')
param appServicePlanName string = 'blazorships-plan'

resource rg 'Microsoft.Resources/resourceGroups@2024-03-01' = {
  name: resourceGroupName
  location: location
}

module webapp 'webapp.bicep' = {
  name: 'webapp-deployment'
  scope: rg
  params: {
    location: location
    webAppName: webAppName
    appServicePlanName: appServicePlanName
  }
}

output webAppName string = webapp.outputs.webAppName
output webAppUrl string = webapp.outputs.webAppUrl
