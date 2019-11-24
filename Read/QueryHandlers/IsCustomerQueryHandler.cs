using System.Linq;
using Persistance;
using Read.Queries;

namespace Read
{
    public class IsCustomerQueryHandler : IQueryHandler<EmailQuery, bool>
    {
        public bool Handle(EmailQuery query)
        {
            return AccountStore.AccountStates.Any(x => x.Email == query.Email);
        }
    }
}