using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Brixel.Soundboard.Web.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tewr.Blazor.FileReader;

namespace Brixel.Soundboard.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            var baseAddress = builder.Configuration["baseAddress"];

            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(baseAddress) });
            builder.Services.AddTransient<ISoundboardProxy, SoundboardProxy>();
            builder.Services.AddFileReaderService(o => o.UseWasmSharedBuffer = true);
            await builder.Build().RunAsync();
        }
    }
}
