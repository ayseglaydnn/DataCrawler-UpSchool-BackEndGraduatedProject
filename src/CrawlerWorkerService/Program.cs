using CrawlerWorkerService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();

        services.AddSingleton<HttpClient>(new HttpClient() { BaseAddress = new Uri("https://localhost:7005/api/") });
    })
    .Build();

host.Run();
