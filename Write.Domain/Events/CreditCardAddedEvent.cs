using System;

namespace Write.Domain.Events
{
    public class CreditCardAddedEvent : IAccountEvent
    {
        public string CreditCard { get; }
        public DateTimeOffset Timestamp { get; set; }

        public CreditCardAddedEvent(string creditCard)
        {
            CreditCard = creditCard;
            Timestamp = DateTimeOffset.Now;
        }
    }
}