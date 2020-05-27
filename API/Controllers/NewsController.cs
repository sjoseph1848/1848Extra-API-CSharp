using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Extra.Models.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _config;

        public NewsController(IHttpClientFactory clientFactory, IConfiguration config)
        {
            _clientFactory = clientFactory;
            _config = config;
        }

        [HttpGet]
        public async Task<IActionResult> GetNews()
        {
            string uri = $"top-headlines?country=us&category=business&apiKey={_config["NewsKey"]}";
            var client = _clientFactory.CreateClient(
                name: "NewsService");
            var request = new HttpRequestMessage(
                method: HttpMethod.Get, requestUri: uri);

            HttpResponseMessage response = await client.SendAsync(request);

            string jsonString = await response.Content.ReadAsStringAsync();

            NewsDto model = JsonConvert.DeserializeObject<NewsDto>(jsonString);

            return Ok(model);
        }

        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetCategory(string category)
        {
            string uri = $"top-headlines?country=us&category={category}&apiKey={_config["NewsKey"]}";
            var client = _clientFactory.CreateClient(
                name: "NewsService");
            var request = new HttpRequestMessage(
                method: HttpMethod.Get, requestUri: uri);

            HttpResponseMessage response = await client.SendAsync(request);

            string jsonString = await response.Content.ReadAsStringAsync();

            NewsDto model = JsonConvert.DeserializeObject<NewsDto>(jsonString);

            return Ok(model);
        }
    }
}