using System.Linq;
using Write.Commands;
using Write.Domain;

namespace Write.CommandHandlers
{
    public class BecomeCustomerCommandHandler : ICommandHandler<BecomeCustomerCommand>
    {
        public void Handle(BecomeCustomerCommand command)
        {
            var state = AccountStore.AccountStates.SingleOrDefault(x => x.Email == command.Email) ?? new AccountState();

            var agg = Account.Create(state);

            agg.BecomeCustomer(command.Email, command.Username);

            foreach (var change in agg.Changes())
            {
                EventStore.Add(change);
            }

            AccountStore.AccountStates.Add(state);
        }
    }
}
