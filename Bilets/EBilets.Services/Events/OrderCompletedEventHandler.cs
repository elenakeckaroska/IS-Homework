using EBilets.Domain.DomainModels;

namespace EBilets.Web.Events
{
    public delegate void OrderCompletedEventHandler(Order completedOrder);
    
}
