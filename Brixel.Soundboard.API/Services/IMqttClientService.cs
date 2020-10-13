using System.IO;
using System.Threading.Tasks;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Receiving;

namespace Brixel.Soundboard.API.Services
{
    public interface IMqttClientService : 
        IMqttClientConnectedHandler,
        IMqttClientDisconnectedHandler,
        IMqttApplicationMessageReceivedHandler
    {
        Task Publish(MemoryStream memoryStream);
    }
}