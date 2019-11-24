using System;
using System.Linq;
using Persistance;
using Read.Queries;

namespace Read
{
    public class HasSubscriptionQueryHandler : IQueryHandler<EmailQuery, bool>
    {
        public bool Handle(EmailQuery query)
        {
            var account = AccountStore.AccountStates.SingleOrDefault(x => x.Email == query.Email);

            return DateTimeOffset.Now >= account?.ActiveSubscription?.ActiveFrom &&
                   DateTimeOffset.Now <= account?.ActiveSubscription?.ActiveTo;
        }
    }
}