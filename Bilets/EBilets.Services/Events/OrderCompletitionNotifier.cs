using DocumentFormat.OpenXml.Office2021.DocumentTasks;
using EBilets.Domain.DomainModels;
using EBilets.Services.Interface;

namespace EBilets.Web.Events
{
    public class OrderCompletionNotifier
    {
        public event OrderCompletedEventHandler OrderCompleted;

        private readonly IBackgroundEmailSender _backgroundEmailSender;

        public OrderCompletionNotifier(IBackgroundEmailSender backgroundEmailSender)
        {
            _backgroundEmailSender = backgroundEmailSender;
        }

        public async void NotifyOrderCompleted(Order completedOrder)
        {
            // Raise the OrderCompleted event
            OrderCompleted?.Invoke(completedOrder);

            // Start the background worker to send emails
            await _backgroundEmailSender.DoWork();

  
        }
    }

}
