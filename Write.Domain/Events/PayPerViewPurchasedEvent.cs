using System;

namespace Write.Domain.Events
{
    public class PayPerViewPurchasedEvent : IAccountEvent
    {
        public Guid MovieId { get; }
        public DateTimeOffset Timestamp { get; set; }

        public PayPerViewPurchasedEvent(Guid movieId)
        {
            MovieId = movieId;
            Timestamp = DateTimeOffset.Now;
        }
    }
}