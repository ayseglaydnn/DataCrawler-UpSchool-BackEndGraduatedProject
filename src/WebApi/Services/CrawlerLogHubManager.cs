using Application.Common.Interfaces;
using Application.Common.Models.CrawlerLog;
using Domain.Utilities;
using Microsoft.AspNetCore.SignalR;
using WebApi.Hubs;

namespace WebApi.Services
{
    public class CrawlerLogHubManager : ICrawlerLogHubService
    {
        private readonly IHubContext<CrawlerLogHub> _hubContext;

        public CrawlerLogHubManager(IHubContext<CrawlerLogHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendLogNotificationAsync(CrawlerLogDto log, CancellationToken cancellationToken)
        {
            await _hubContext.Clients.All.SendAsync(SignalRMethodKeys.CreawlerLog.NewCrawlerLogAdded, log);
        }
    }
}
