using System;
using System.Collections.Generic;
using System.Linq;

namespace Persistance
{
    public class AccountStore
    {
        public static List<Account> AccountStates = new List<Account>();

        public static void Update(Account account)
        {
            AccountStates = AccountStates.Where(x => x.Email != account.Email).ToList();

            AccountStates.Add(account);
        }
    }

    public class Account
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public List<Guid> PayPerViews { get; set;  }
        public List<CreditCard> CreditCards { get; set; }
        public Subscription ActiveSubscription { get; set; }

        public class CreditCard
        {
            public string Number { get; set; }
            public bool IsPrimary { get; set; }
        }

        public class Subscription
        {
            public DateTimeOffset ActiveFrom { get; set; }
            public DateTimeOffset ActiveTo { get; set; }
        }
    }
}