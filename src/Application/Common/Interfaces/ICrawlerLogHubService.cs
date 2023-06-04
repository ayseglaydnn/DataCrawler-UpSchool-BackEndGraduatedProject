using Application.Common.Models.CrawlerLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface ICrawlerLogHubService
    {
        Task SendLogNotificationAsync(CrawlerLogDto log, CancellationToken cancellationToken);
    }
}
