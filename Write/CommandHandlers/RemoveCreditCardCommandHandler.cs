using System;
using System.Linq;
using Write.Commands;
using Write.Domain;

namespace Write.CommandHandlers
{
    public class RemoveCreditCardCommandHandler : ICommandHandler<RemoveCreditCardCommand>
    {
        public void Handle(RemoveCreditCardCommand command)
        {
            var state = AccountStore.AccountStates.SingleOrDefault(x => x.Email == command.Email);

            if (state == null)
            {
                throw new Exception("Not found");
            }

            var agg = Account.Create(state);

            agg.RemoveCreditCard(command.Creditcard);

            foreach (var change in agg.Changes())
            {
                EventStore.Add(change);
            }
        }
    }
}