namespace Brixel.Soundboard.Client.Services
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

    public class Player
    {
        public Player()
        {
            Effects = new List<SoundEffect>();
        }
        public List<SoundEffect> Effects { get; set; }
    }

    public class SoundEffect
    {
        public string Effect { get; set; }
        public string File { get; set; }
    }
}