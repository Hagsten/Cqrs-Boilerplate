using System.Collections.Generic;
using Write.Domain;

namespace Write
{
    public static class EventStore
    {
        private static readonly List<IAccountEvent> Events = new List<IAccountEvent>();

        public static void Add(IAccountEvent e)
        {
            Events.Add(e);
        }

        public static IReadOnlyCollection<IAccountEvent> GetHistory()
        {
            return Events.AsReadOnly();
        }
    }
}
