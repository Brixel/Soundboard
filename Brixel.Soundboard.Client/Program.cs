// See https://aka.ms/new-console-template for more information
using Brixel.Soundboard.Client.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Hello, World!");

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables().Build();
    
var services = new ServiceCollection();
var mqttOptions = configuration.GetSection(nameof(MqttOptions));
services.Configure<MqttOptions>(mqttOptions);
var options = mqttOptions.Get<MqttOptions>();

var playerOptions = configuration.GetSection(nameof(Player)).Get<Player>();

var mqttClient = new MqttClientService(options, playerOptions);
await mqttClient.Subscribe();
Console.WriteLine($"Client is listening for messages on: {Environment.NewLine}" +
    $"Server: {options.Server}{Environment.NewLine}" +
    $"Topic: {options.Topic}");
Console.ReadLine();