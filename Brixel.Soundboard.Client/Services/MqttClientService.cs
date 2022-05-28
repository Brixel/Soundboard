using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;

namespace Brixel.Soundboard.Client.Services
{
    public class MqttClientService : IMqttClientService
    {
        //private readonly ILogger<MqttClientService> _logger;
        private IMqttClient _client;
        private IMqttClientOptions _options;
        private MqttFactory _factory;
        private readonly string _topic;
        private readonly Player _player;

        public MqttClientService(IOptions<MqttOptions> mqttOptions, IOptions<Player> player)
        {
            //_logger = logger;
            var mqtt = mqttOptions.Value;
            _topic = mqtt.Topic;

            _player = player.Value;

            CreateMqttClient(mqtt);

            DiscoverEffects();
            //DiscoverOutput();
        }

        private void DiscoverEffects()
        {
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Listing effects");
            var effects = _player.Effects;
            foreach(var effect in effects)
            {
                Console.WriteLine($"{effect.Effect} is located at {effect.File}");
            }
        }

        public async Task Subscribe()
        {

            await _client.ConnectAsync(_options);

            _client.UseApplicationMessageReceivedHandler(msg =>
            {
                byte[]? payload = msg?.ApplicationMessage?.Payload ?? Array.Empty<byte>();
                var text = Encoding.UTF8.GetString(payload);
                ProcessPayload(text);
            });

            var mqttSubscribeOptions = _factory.CreateSubscribeOptionsBuilder()
                .WithTopicFilter(f => f.WithTopic(_topic)).Build();
            var response = await _client.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);

            
            Console.WriteLine(response);
        }

        private void ProcessPayload(string payload)
        {
            Console.WriteLine($"Processing {payload}");
            string program = "vlc.exe";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                program = "cvlc";

            var effect = _player.Effects.SingleOrDefault(x => x.Effect == payload);

            if(effect == null)
            {
                Console.WriteLine($"No file found for effect {payload}");
                return;
            }
            Console.WriteLine($"Sending {effect.File} to VLC to play {effect.Effect}");
            var pi = new ProcessStartInfo(effect.File)
            {
                Arguments = Path.GetFileName(effect.File) + " --play-and-exit",
                UseShellExecute = true,
                WorkingDirectory = Path.GetDirectoryName(effect.File),
                FileName = program,
                Verb = "OPEN",
                WindowStyle = ProcessWindowStyle.Hidden
            };

            Process p = new();
            p.StartInfo = pi;
            p.Start();
            p.WaitForExit();
        }

        


        private void CreateMqttClient(MqttOptions mqtt)
        {
            var mqttClientOptionsBuilder = new MqttClientOptionsBuilder()
                .WithClientId(mqtt.ClientId)
                .WithTcpServer(mqtt.Server, mqtt.Port);
            if (!string.IsNullOrEmpty(mqtt.Username))
            {
                mqttClientOptionsBuilder.WithCredentials(mqtt.Username, mqtt.Password);
            }

            _options = mqttClientOptionsBuilder
                .WithCleanSession()
                .Build();

            _factory = new MqttFactory();
            _client = _factory.CreateMqttClient();
            _client.ConnectedHandler = this;
            _client.DisconnectedHandler = this;
        }

        public async Task Publish(MemoryStream memoryStream)
        {
            await _client.ConnectAsync(_options);
            var byteArray = memoryStream.ToArray();
            await _client.PublishAsync(_topic, byteArray);
        }

        public Task HandleConnectedAsync(MqttClientConnectedEventArgs eventArgs)
        {
            //_logger.LogInformation("MQTT Client connected");
            return Task.CompletedTask;
        }

        public Task HandleDisconnectedAsync(MqttClientDisconnectedEventArgs eventArgs)
        {
            //_logger.LogInformation("MQTT Client disconnected");
            return Task.CompletedTask;
        }

        public Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            //_logger.LogInformation("MQTT Message: {message}", eventArgs.ApplicationMessage);
            throw new NotImplementedException();
        }
    }
}
