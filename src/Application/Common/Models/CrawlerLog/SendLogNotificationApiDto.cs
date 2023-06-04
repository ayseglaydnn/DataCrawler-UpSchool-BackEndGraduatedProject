using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Models.CrawlerLog
{
    public class SendLogNotificationApiDto
    {
        public CrawlerLogDto Log { get; set; }
        public string ConnectionId { get; set; }

        public SendLogNotificationApiDto(CrawlerLogDto log, string connectionId)
        {
            Log = log;
            ConnectionId = connectionId;
        }
    }
}
