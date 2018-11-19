using RevolutionGolf.EventDto;
using RevolutionGolf.EventDto.Billing;
using RevolutionGolf.EventRequestBroker.EventServices;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RevolutionGolf.EventServices.Billing
{
    public class BillingSubscriptionService : IEventService
    {
        public bool CanConsumeMessage(Type type)
        {
            return type == typeof(BillingAgreementDto);
        }

        public async Task<HttpStatusCode> HandleEvent(NotificationMessageDto message)
        {
            if (message.EventType == "")
            {
                return HttpStatusCode.BadRequest;
            }
            else
            {
                return HttpStatusCode.OK;
            }
        }

        private async Task SubscriptionAcivated(NotificationMessageDto message)
        {
            return;
        }

        private async Task SubscriptionCancelled(NotificationMessageDto message)
        {
            return;
        }

        private async Task SubscriptionSuspended(NotificationMessageDto message)
        {
            return;
        }
    }
}
