using RevolutionGolf.EventDto;
using RevolutionGolf.EventDto.Payments;
using RevolutionGolf.EventRequestBroker.EventServices;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RevolutionGolf.EventServices.Payments
{
    public class AuthorizationService : IEventService
    {
        public bool CanConsumeMessage(Type type) => type == typeof(AuthorizationDto);

        public async Task<HttpStatusCode> HandleEvent(NotificationMessageDto message)
        {
            if (message.EventType != "")
            {
                return HttpStatusCode.OK;
            }
            else
            {
                return HttpStatusCode.BadRequest;
            }
        }
    }
}
