
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface INotificationHubService
    {
        Task SendAppNotificationAsync(string notification, CancellationToken cancellationToken);
    }
}
