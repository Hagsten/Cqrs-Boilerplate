using System;

namespace Write.Domain.Events
{
    public class BecameCustomerEvent : IAccountEvent
    {
        public string Email { get; }
        public string Username { get; }

        public BecameCustomerEvent(string email, string username)
        {
            Email = email;
            Username = username;
            Timestamp = DateTimeOffset.Now;
        }

        public DateTimeOffset Timestamp { get; set; }
    }
}