using System;

namespace Write.Domain.Events
{
    public class CreditCardRemovedEvent : IAccountEvent
    {
        public string CreditCard { get; }
        public DateTimeOffset Timestamp { get; set; }

        public CreditCardRemovedEvent(string creditCard)
        {
            CreditCard = creditCard;
            Timestamp = DateTimeOffset.Now;
        }
    }
}