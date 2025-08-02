# Refreshing Key Vault Example

At Resonite, we wanted to transition to using Azure Key Vault for our secrets. 
One of the key features of this initiative was the ability to change secrets without restarting the application.

By Combining together some packages:
- [Extensions.Options.AutoBinder](https://www.nuget.org/packages/Extensions.Options.AutoBinder)
- [Azure.Extensions.AspNetCore.Configuration.Secrets](https://www.nuget.org/packages/Azure.Extensions.AspNetCore.Configuration.Secrets)

and ensuring that the `ReloadInterval` is set for the Secret Store. 

We're able to use [IOptionsMonitor](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.options.ioptionsmonitor-1?view=net-9.0-pp) to deliver a POCO(Plain old CLR object), 
to an `Microsoft.Extensions.Hosting.BackgroundService` instance, in a really clean way.

The service can then just update the underlying Secret Consumer, which in this case is a [Patreon.NET Client](https://www.nuget.org/packages/YellowDogMan.Patreon.NET) for example reasons.

## Disclaimer
This is a sample project, for demonstration purposes. No production secrets are ever shared or inserted into this app.

Additionally before implementing this approach in your application ensure you read [Azure's Best Practices for KeyVault](https://learn.microsoft.com/en-us/azure/key-vault/general/best-practices).

Oh and NEVER, log secrets to a logger like we do here. Follow [Log Redaction](https://learn.microsoft.com/en-us/dotnet/core/extensions/data-redaction?tabs=dotnet-cli#example-redacting-data-in-logs) guidance.

## Setup
1. Create an Azure Key Vault
1. Create Secrets in the vault:. 
    - `DummyPatreonKeys--CampaignId` - Campaign for Patreon.NET maps to DummyPatreonKeys.CampaignId
    - `DummyPatreonKeys--AccessToken` - Acces Token for Patreon.NET maps to DummyPatreonKeys.AccessToken
    - See [this page](https://learn.microsoft.com/en-us/aspnet/core/security/key-vault-configuration?view=aspnetcore-9.0&viewFallbackFrom=aspnetcore-2.2#bind-an-array-to-a-class) for more information on binding.
1. Create an Azure App Service that you can deploy this code to
1. Ensure the App Service has a [Managed Identity, with permissions on the vault](https://learn.microsoft.com/en-us/azure/key-vault/general/tutorial-net-create-vault-azure-web-app?tabs=azure-cli#create-and-assign-access-to-a-managed-identity)
1. Deploy
1. Observe the Logs, they should pick up the initial value
1. Modify the Secret Value
1. Wait 1 Minute
1. Observe the Logs, They should pickup the new keys

## Video Explainer

[A video explaining this project is available](https://www.youtube.com/watch?v=AzuIqyyVSTg)

## References
- https://learn.microsoft.com/en-us/azure/key-vault/general/overview
- https://learn.microsoft.com/en-us/azure/key-vault/general/tutorial-net-create-vault-azure-web-app?tabs=azure-cli
- https://learn.microsoft.com/en-us/dotnet/api/azure.extensions.aspnetcore.configuration.secrets.azurekeyvaultconfigurationoptions.reloadinterval?view=azure-dotnet
- https://www.youtube.com/watch?v=CZlls_IFL-Q
- https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.options.ioptionsmonitor-1?view=net-9.0-pp
- https://learn.microsoft.com/en-us/aspnet/core/security/key-vault-configuration?view=aspnetcore-9.0&viewFallbackFrom=aspnetcore-2.2#bind-an-array-to-a-class
- https://learn.microsoft.com/en-us/dotnet/core/extensions/data-redaction?tabs=dotnet-cli#example-redacting-data-in-logs
- https://github.com/Azure/azure-sdk-for-net/tree/main/sdk/extensions/Azure.Extensions.AspNetCore.Configuration.Secrets/src
