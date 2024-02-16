namespace AsyncSemanticKernelResponse.Services
{
    public class ConfigurationService
    {
        private readonly HttpClient _httpClient;

        public ConfigurationService(HttpClient httpClient) =>
            _httpClient = httpClient;
        async public Task<string> GetPromptAsync()
        {
            return await _httpClient.GetStringAsync("/assets/prompt.txt");
        }
        async public Task<string> GetSentenceAsync()
        {
            return await _httpClient.GetStringAsync("/assets/sentence.txt");
        }
    }
}
