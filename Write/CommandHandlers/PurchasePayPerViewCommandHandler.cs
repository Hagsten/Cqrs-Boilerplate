using System;
using System.Linq;
using Persistance;
using Write.Commands;
using Write.Domain;
using Account = Write.Domain.Account;

namespace Write.CommandHandlers
{
    public class PurchasePayPerViewCommandHandler : ICommandHandler<PurchasePayPerViewCommand>
    {
        public void Handle(PurchasePayPerViewCommand command)
        {
            var state = AccountState.FromStorage(AccountStore.AccountStates.SingleOrDefault(x => x.Email == command.Email));

            if (state == null)
            {
                throw new Exception("Not found");
            }

            var agg = Account.Create(state);

            agg.PurchasePayPerView(command.MovieId);

            foreach (var change in agg.Changes())
            {
                EventStore.Add(change);
            }

            AccountStore.Update(state.ToStorage());
        }
    }
}