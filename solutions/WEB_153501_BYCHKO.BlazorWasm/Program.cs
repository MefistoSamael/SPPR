using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using WEB_153501_BYCHKO.BlazorWasm;
using WEB_153501_BYCHKO.BlazorWasm.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var ApiUri = builder.Configuration.GetSection("UriData:ApiUri").Value;

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(ApiUri!) });


builder.Services.AddScoped<IDataService, _dataService>();

builder.Services.AddOidcAuthentication(options =>
{
    // Configure your authentication provider options here.
    // For more information, see https://aka.ms/blazor-standalone-auth
    builder.Configuration.Bind("Local", options.ProviderOptions);
});

await builder.Build().RunAsync();
