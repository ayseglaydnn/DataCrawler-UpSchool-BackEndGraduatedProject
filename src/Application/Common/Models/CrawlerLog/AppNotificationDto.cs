using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Models.CrawlerLog
{
    public class AppNotificationDto
    {
        public int? Id { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public DateTimeOffset SentOn { get; set; }
        
    }
}
