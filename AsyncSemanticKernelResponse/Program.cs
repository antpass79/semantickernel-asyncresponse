using AsyncSemanticKernelResponse.Components;
using AsyncSemanticKernelResponse.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7237") });
builder.Services.AddScoped<ConfigurationService>();
builder.Services.AddScoped<KernelStreamingService>();
builder.Services.AddScoped<JsonAsyncStreamingService>();

builder.Services.AddSingleton(factory =>
{
    var kernel = Kernel.CreateBuilder()
                .AddAzureOpenAIChatCompletion(
                    "",                             // Azure OpenAI Deployment Name
                    "",                             // Azure OpenAI Endpoint
                    "",                             // Key
                    httpClient: new HttpClient()
                )
                .Build();

    return kernel;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(AsyncSemanticKernelResponse.Client._Imports).Assembly);

app.MapGet("/commands", async (
    [FromServices] KernelStreamingService kernelStreamingService,
    [FromServices] ConfigurationService configurationService) =>
{
    var prompt = await configurationService!.GetPromptAsync();
    var sentence = await configurationService!.GetSentenceAsync();

    return kernelStreamingService.StreamingAsync(prompt, sentence);
});

app.Run();
