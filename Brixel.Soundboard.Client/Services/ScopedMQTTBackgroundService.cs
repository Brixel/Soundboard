using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Brixel.Soundboard.Client.Services
{
    public class ScopedMQTTBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public ScopedMQTTBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var scopedMqttClient = scope.ServiceProvider.GetService<IMqttClientService>();
            await scopedMqttClient.Subscribe();
        }
    }
}
