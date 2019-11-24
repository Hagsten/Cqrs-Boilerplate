using System;
using System.Linq;
using Persistance;
using Read.Queries;

namespace Read.QueryHandlers
{
    public class CanViewQueryHandler : IQueryHandler<PayPerViewQuery, bool>
    {
        public bool Handle(PayPerViewQuery query)
        {
            var account = AccountStore.AccountStates.SingleOrDefault(x => x.Email == query.Email);

            var ppw = account?.PayPerViews.SingleOrDefault(x => x == query.MovieId);

            return ppw != Guid.Empty;
        }
    }
}