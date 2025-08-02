using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Extensions.Options.AutoBinder;
using Microsoft.Extensions.Options;
using RefreshingKeyVaultExample;

var builder = WebApplication.CreateBuilder();

builder.Services.AddLogging(builder => builder.AddConsole());

if (builder.Environment.IsProduction())
    builder.Configuration.AddAzureKeyVault(
    new Uri(builder.Configuration["KeyVaultUrl"]!),
    new ManagedIdentityCredential(),
    new AzureKeyVaultConfigurationOptions
    {
        ReloadInterval = TimeSpan.FromMinutes(1)
    });

builder.Services.AddOptions<DummyPatreonKeys>().AutoBind();
builder.Services.AddHostedService<Worker>();

var app = builder.Build();

app.MapGet("/health", (IOptionsSnapshot<DummyPatreonKeys> keysI) =>
{
    var keys = keysI.Value;
    // Also do not do this!
    return Results.Ok($"Current keys are: {keys.CampaignId}, {keys.AccessToken}");
});

await app.RunAsync();
