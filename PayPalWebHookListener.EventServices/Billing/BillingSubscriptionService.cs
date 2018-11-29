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
        public bool CanConsumeMessage(Type type) => type == typeof(BillingAgreementDto);

        public async Task<HttpStatusCode> HandleEvent(NotificationMessageDto message)
        {
            switch (message.EventType)
            {
                case "BILLING.SUBSCRIPTION.CANCELLED":
                    return await SubscriptionCancelled(message);
                case "BILLING.SUBSCRIPTION.ACTIVATED":
                    return await SubscriptionCancelled(message);
                case "BILLING.SUBSCRIPTION.SUSPENDED":
                    return await SubscriptionCancelled(message);
                default:
                    return HttpStatusCode.NotFound;
            }
        }

        private async Task<HttpStatusCode> SubscriptionAcivated(NotificationMessageDto message)
        {
            return HttpStatusCode.OK;
        }

        private async Task<HttpStatusCode> SubscriptionCancelled(NotificationMessageDto message)
        {
            return HttpStatusCode.OK;
        }

        private async Task<HttpStatusCode> SubscriptionSuspended(NotificationMessageDto message)
        {
            return HttpStatusCode.OK;
        }
    }
}
