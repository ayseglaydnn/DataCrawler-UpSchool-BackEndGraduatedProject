using Application.Common.Interfaces;
using Application.Common.Models.Order;
using Application.Features.Orders.Queries.GetById;
using Domain.Utilities;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.VisualBasic;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace CrawlerWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ICrawlerService _crawlerService;
        private readonly ILogger<Worker> _logger;
        private readonly HubConnection _connection;
        private readonly HttpClient _httpClient;

        public Worker(ICrawlerService crawlerService, ILogger<Worker> logger, HttpClient httpClient)
        {
            _crawlerService = crawlerService;
            _logger = logger;
            _httpClient = httpClient;

            _connection = new HubConnectionBuilder()
                .WithUrl($"https://localhost:7005/Hubs/OrderHub")
                .WithAutomaticReconnect()
                .Build();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _connection.On<CrawlerWorkerServiceOrderDto>("NewOrderAdded", async ( orderAddDto ) =>
            {
                Console.WriteLine($"A new order added. Requested amount is {orderAddDto.Order.RequestedAmount}.");
                Console.WriteLine($"Acces Token is {orderAddDto.AccessToken}.");

                var websiteUrl = "https://4teker.net/";

                var scrapedProducts = await _crawlerService.ScrapeWebsiteAsync(websiteUrl, orderAddDto.Order.Id.ToString(), orderAddDto.Order.RequestedAmount, orderAddDto.Order.ProductCrawlType, stoppingToken);

                await Task.Delay(10000, stoppingToken);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", orderAddDto.AccessToken);

                var result = await _httpClient.PostAsJsonAsync("Orders/CrawlerWorkerService", orderAddDto, stoppingToken);

            });

            await _connection.StartAsync(stoppingToken);

            Console.WriteLine(_connection.State.ToString());
            Console.WriteLine(_connection.ConnectionId);


            while (!stoppingToken.IsCancellationRequested)
            {
                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                //await Task.Delay(1000, stoppingToken);
            }
        }
    }
}