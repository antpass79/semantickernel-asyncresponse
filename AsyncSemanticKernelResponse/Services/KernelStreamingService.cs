using Microsoft.SemanticKernel;

namespace AsyncSemanticKernelResponse.Services
{
    public class KernelStreamingService(Kernel kernel, JsonAsyncStreamingService jsonAsyncStreamingService)
    {
        async public IAsyncEnumerable<string> StreamingAsync(string prompt, string sentence)
        {
            await foreach (var chunk in kernel.InvokePromptStreamingAsync<string>(prompt, arguments: new()
            {
                { "input", sentence }
            }))
            {
                var json = jsonAsyncStreamingService.ReadNextJson(chunk);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    await Task.Delay(3000);
                    yield return json;
                }
            }
        }
    }
}
