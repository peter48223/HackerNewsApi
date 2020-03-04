using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace HackerNewsApi
{
    public class Http : IHttp
    {
        private readonly ILogger<Http> _logger;
        private readonly IHttpClientFactory _clientFactory;

        public Http(ILogger<Http> logger,
            IHttpClientFactory clientFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); ;
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory)); ;
        }

        public async Task<int[]> GetStories()
        {
            var url = "https://hacker-news.firebaseio.com/v0/topstories.json?print=pretty";

            var response = await MakeHttpCall(url);

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var storyIdsFromHackerNews = await System.Text.Json.JsonSerializer.DeserializeAsync
                <JsonElement>(responseStream);

            return GetArray(storyIdsFromHackerNews);
        }

        public async Task<Story> GetStory(int id)
        {
            // todo: put in config
            var url = $"https://hacker-news.firebaseio.com/v0/item/{id}.json?print=pretty";

            var response = await MakeHttpCall(url);

            using var storyStream = await response.Content.ReadAsStreamAsync();
            var storyElement = await System.Text.Json.JsonSerializer.DeserializeAsync
                <JsonElement>(storyStream);

            return JsonConvert.DeserializeObject<Story>(storyElement.ToString());
        }

        private async Task<HttpResponseMessage> MakeHttpCall(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Access-Control-Allow-Origin", "http://localhost:4200");
            var client = _clientFactory.CreateClient();
            return await client.SendAsync(request);
        }

        private int[] GetArray(JsonElement storyIds)
        {

            var x = storyIds.ToString();
            var y = x.Substring(1, x.Length - 2);
            var z = y.Split(',');

            return Array.ConvertAll(z, s => int.Parse(s));
        }
    }
}
