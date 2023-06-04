using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Utilities
{
    public static class SignalRMethodKeys
    {
        public static class Orders
        {
            public static string OrderAdded => nameof(OrderAdded);
        }
        public static class CreawlerLog
        {
            public static string NewCrawlerLogAdded => nameof(NewCrawlerLogAdded);
        }
        public static class Notification
        {
            public static string AppNotificationSended => nameof(AppNotificationSended);
        }
    }
}
