using System;
using System.Linq;
using Write.Commands;
using Write.Domain;

namespace Write.CommandHandlers
{
    public class PromoteCreditCardAsPrimaryCommandHandler : ICommandHandler<PromoteCreditCardAsPrimaryCommand>
    {
        public void Handle(PromoteCreditCardAsPrimaryCommand command)
        {
            var state = AccountStore.AccountStates.SingleOrDefault(x => x.Email == command.Email);

            if (state == null)
            {
                throw new Exception("Not found");
            }

            var agg = Account.Create(state);

            agg.PromoteCreditCard(command.CreditCard);

            foreach (var change in agg.Changes())
            {
                EventStore.Add(change);
            }
        }
    }
}