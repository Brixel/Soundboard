﻿using System.IO;
using System.Threading.Tasks;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Receiving;

namespace Brixel.Soundboard.Client.Services
{
    public interface IMqttClientService : 
        IMqttClientConnectedHandler,
        IMqttClientDisconnectedHandler,
        IMqttApplicationMessageReceivedHandler
    {
        Task Publish(MemoryStream memoryStream);

        Task Subscribe();
    }
}