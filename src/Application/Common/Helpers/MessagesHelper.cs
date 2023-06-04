using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Helpers
{
    public static class MessagesHelper
    {
        public static class Email
        {
            public static class Information
            {
                public static string Subject => "Thank you for choosing this App.";
                public static string InformationMessage => "Your order has been completed. For details, click the button given below.";
                public static string ButtonText => "Details";
                public static string ButtonLink => "button_link";
                public static string Name(string firstName) => $"Hi {firstName}";
            }
        }
    }
}
