using System;
using System.Collections.Generic;
using System.Text;
using Write.Domain;

namespace Write
{
    public class AccountStore
    {
        public static List<AccountState> AccountStates = new List<AccountState>();
    }

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
