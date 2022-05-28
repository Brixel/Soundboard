using Brixel.Soundboard.Client.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine("Hello, World!");

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, configuration) =>
    {
        configuration.Sources.Clear();
        IHostEnvironment env = hostingContext.HostingEnvironment;

        configuration
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            //.AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);

        IConfigurationRoot configurationRoot = configuration.Build();

    })
    .ConfigureServices((context, services )=>
    {
        var configurationRoot = context.Configuration;

        var mqttOptions = configurationRoot.GetSection(nameof(MqttOptions));
        var options = mqttOptions.Get<MqttOptions>();
        services.Configure<MqttOptions>(mqttOptions);

        var playerOptions = configurationRoot.GetSection(nameof(Player));
        services.Configure<Player>(playerOptions);
        services.AddTransient<IMqttClientService, MqttClientService>();
        services.AddHostedService<ScopedMQTTBackgroundService>();
        Console.WriteLine($"Client is listening for messages on: {Environment.NewLine}" +
            $"Server: {options.Server}{Environment.NewLine}" +
            $"Topic: {options.Topic}");

    })
    .UseSystemd()
    .Build();
await host.RunAsync();

