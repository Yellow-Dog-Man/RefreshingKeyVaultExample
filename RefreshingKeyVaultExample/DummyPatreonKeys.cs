// This is a sample project, this object will never contain actual keys.
public class DummyPatreonKeys
{
    public string CampaignId { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;

    [Obsolete("Don't do this")]
    public void LogTokens(ILogger logger)
    {
        // See the Disclaimer in the Readme. DO NOT DO THIS.
        // DEMONSTRATION PURPOSES ONLY!!!
        logger.LogInformation("Current Keys for DEMONSTRATION PURPOSES ONLY!!. Campaign: {campaign}. Token: {token}", CampaignId, AccessToken);
        // DEMONSTRATION PURPOSES ONLY!!
        // DO NOT DO!!!
    }
}