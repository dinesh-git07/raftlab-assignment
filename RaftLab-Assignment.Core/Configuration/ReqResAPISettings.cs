namespace RaftLab_Assignment.Configuration
{
    public class ReqResAPISettings
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public int TimeoutSeconds { get; set; } = 30;
        public int MaxRetryAttempts { get; set; } = 3;
    }
}
