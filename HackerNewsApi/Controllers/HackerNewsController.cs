using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HackerNewsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HackerNewsController : ControllerBase
    {
        private readonly ILogger<HackerNewsController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IHttp _http;
        private readonly IMemoryCache _memoryCache;

        string cacheKey = "Stories";

        public HackerNewsController(ILogger<HackerNewsController> logger,
            IHttpClientFactory clientFactory,
            IHttp http,
            IMemoryCache memoryCache)
        {
            _logger = logger;
            _clientFactory = clientFactory;
            _http = http;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public async Task<IEnumerable<Story>> GetAsync()
        {
            var StoryLinks = new List<Story>();
            StoryLinks = _memoryCache.Get(cacheKey) as List<Story>;
            if (StoryLinks == null)
            {
                StoryLinks = new List<Story>();
                var storyIdsArray = await _http.GetStories();
                foreach (var id in storyIdsArray.Take(50))
                {
                    var story = await _http.GetStory(id);
                    if (story.Url?.Length > 7)
                    {
                        StoryLinks.Add(story);
                    }
                }

                _memoryCache.Set(cacheKey, StoryLinks, new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(2)));
            }

            return StoryLinks.AsEnumerable();
        }

    }
}