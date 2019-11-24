using System;

namespace Write.Domain
{
    public interface IAccountEvent
    {
        DateTimeOffset Timestamp { get; set; }
    }
}