using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Brixel.Soundboard.Web.Services
{
    public class SoundboardProxy : ISoundboardProxy
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<SoundboardProxy> _logger;

        public SoundboardProxy(HttpClient httpClient, ILogger<SoundboardProxy> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<bool> UploadProductImage(MultipartFormDataContent content)
        {
            _logger.LogInformation($"Found {content.Count()} files");
            var postResult = await _httpClient.PostAsync("/Sound/play", content);
            var postContent = await postResult.Content.ReadAsStringAsync();
            return postResult.IsSuccessStatusCode;
        }
    }

    public interface ISoundboardProxy
    {
        Task<bool> UploadProductImage(MultipartFormDataContent content);
    }
}
