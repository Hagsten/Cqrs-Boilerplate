using System;

namespace Write.Domain.Events
{
    public class CreditCardDemotedEvent : IAccountEvent
    {
        public string CreditCard { get; }
        public DateTimeOffset Timestamp { get; set; }

        public CreditCardDemotedEvent(string creditCard)
        {
            CreditCard = creditCard;
            Timestamp = DateTimeOffset.Now;
        }
    }
}