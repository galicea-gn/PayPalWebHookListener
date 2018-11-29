using RevolutionGolf.EventDto;
using RevolutionGolf.EventDto.Invoicing;
using RevolutionGolf.EventRequestBroker.EventServices;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RevolutionGolf.EventServices.Invoicing
{
    public class InvoicesService : IEventService
    {
        public bool CanConsumeMessage(Type type) => type == typeof(InvoiceDto);

        public async Task<HttpStatusCode> HandleEvent(NotificationMessageDto message)
        {
            switch (message.EventType)
            {
                case "INVOICING.INVOICE.CREATED":
                    return await InvoiceCreated(message);
                default:
                    return HttpStatusCode.NotFound;
            }
        }

        private async Task<HttpStatusCode> InvoiceCreated(NotificationMessageDto message)
        {
            return HttpStatusCode.OK;
        }
    }
}
