using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Models.CrawlerLog
{
    public class CrawlerLogDto
    {
        public string Message { get; set; }
        public DateTimeOffset SentOn { get; set; }

        public CrawlerLogDto(string message)
        {
            Message = message;
            SentOn = DateTimeOffset.Now;
        }

    }
}
