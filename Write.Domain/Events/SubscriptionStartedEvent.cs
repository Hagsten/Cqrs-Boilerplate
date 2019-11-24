using System;

namespace Write.Domain.Events
{
    public class SubscriptionStartedEvent : IAccountEvent
    {
        public DateTimeOffset Timestamp { get; set; }

        public SubscriptionStartedEvent()
        {
            Timestamp = DateTimeOffset.Now;
        }
    }
}