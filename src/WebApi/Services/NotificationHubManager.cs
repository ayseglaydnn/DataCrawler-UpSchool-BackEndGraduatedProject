using Application.Common.Interfaces;
using Application.Common.Models.CrawlerLog;
using Domain.Utilities;
using Microsoft.AspNetCore.SignalR;
using WebApi.Hubs;

namespace WebApi.Services
{
    public class NotificationHubManager : INotificationHubService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationHubManager(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public Task SendAppNotificationAsync(AppNotificationDto notificationDto, CancellationToken cancellationToken)
        {
            return _hubContext.Clients.All.SendAsync(SignalRMethodKeys.Notification.AppNotificationSended, notificationDto);
        }
    }
}
