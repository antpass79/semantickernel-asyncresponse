namespace AsyncSemanticKernelResponse.Client.Services
{
    public class ConfigurationService(HttpClient httpClient)
    {
        async public Task<string> GetPromptAsync()
        {
            return await httpClient.GetStringAsync($"/prompt.txt");
        }
        async public Task<string> GetSentenceAsync()
        {
            return await httpClient.GetStringAsync($"/sentence.txt");
        }
    }
}
