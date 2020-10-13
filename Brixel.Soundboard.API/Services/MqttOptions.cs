namespace Brixel.Soundboard.API.Services
{
    public class MqttOptions
    {
        public string ClientId { get; set; }
        public string Server { get; set; }
        public int? Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Topic { get; set; }
    }
}