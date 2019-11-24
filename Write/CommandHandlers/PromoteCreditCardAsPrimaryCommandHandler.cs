using System;
using System.Linq;
using Persistance;
using Write.Commands;
using Write.Domain;
using Account = Write.Domain.Account;

namespace Write.CommandHandlers
{
    public class PromoteCreditCardAsPrimaryCommandHandler : ICommandHandler<PromoteCreditCardAsPrimaryCommand>
    {
        public void Handle(PromoteCreditCardAsPrimaryCommand command)
        {
            var state = AccountState.FromStorage(AccountStore.AccountStates.SingleOrDefault(x => x.Email == command.Email));

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

            AccountStore.Update(state.ToStorage());
        }
    }
}