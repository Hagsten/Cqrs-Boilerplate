using System;

namespace Write.Domain.Events
{
    public class CreditCardPromotedEvent : IAccountEvent
    {
        public string CreditCard { get; }
        public DateTimeOffset Timestamp { get; set; }

        public CreditCardPromotedEvent(string creditCard)
        {
            CreditCard = creditCard;
            Timestamp = DateTimeOffset.Now;
        }
    }
}