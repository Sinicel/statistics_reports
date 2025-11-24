using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using statistics_reports;
using statistics_reports.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Регистрируем наши сервисы
builder.Services.AddScoped<SupabaseSession>();
builder.Services.AddScoped<SupabaseClient>();

await builder.Build().RunAsync();
