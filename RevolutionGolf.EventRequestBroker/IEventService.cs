using Microsoft.AspNetCore.Http;
using RevolutionGolf.EventDto;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RevolutionGolf.EventRequestBroker.EventServices
{
    public interface IEventService
    {
        Task<HttpStatusCode> HandleEvent(NotificationMessageDto message);
        bool CanConsumeMessage(Type type);
    }
}
