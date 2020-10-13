using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;

namespace Brixel.Soundboard.API.Services
{
    public class MqttClientService : IMqttClientService
    {
        private readonly ILogger<MqttClientService> _logger;
        private IMqttClient _client;
        private IMqttClientOptions _options;
        private readonly string _topic;

        public MqttClientService(ILogger<MqttClientService> logger, IOptions<MqttOptions> mqttOptions)
        {
            _logger = logger;
            var mqtt = mqttOptions.Value;
            _topic = mqtt.Topic;

            CreateMqttClient(mqtt);
            ConfigureMQTTHandlers();
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
            var factory = new MqttFactory();
            _client = factory.CreateMqttClient();
        }

        private void ConfigureMQTTHandlers()
        {
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
            _logger.LogInformation("MQTT Client connected");
            return Task.CompletedTask;
        }

        public Task HandleDisconnectedAsync(MqttClientDisconnectedEventArgs eventArgs)
        {
            _logger.LogInformation("MQTT Client disconnected");
            return Task.CompletedTask;
        }

        public Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            throw new NotImplementedException();
        }
    }
}
