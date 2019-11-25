using System;
using System.Linq;
using Persistance;
using Write.Domain;
using Account = Write.Domain.Account;

namespace Write
{
    public class CommandProcess
    {
        private readonly AccountState _state;
        private readonly Account _account;

        private CommandProcess(AccountState state, Account account)
        {
            _state = state;
            _account = account;
        }

        public static CommandProcess Start(string email)
        {
            var state = AccountState.FromStorage(AccountStore.AccountStates.SingleOrDefault(x => x.Email == email));

            if (state == null)
            {
                throw new Exception("Not found");
            }

            var account = Account.Create(state);

            return new CommandProcess(state, account);
        }

        public CommandProcess Execute(Action<Account> worker)
        {
            worker(_account);

            return this;
        }

        public void Complete()
        {
            foreach (var change in _account.Changes())
            {
                EventStore.Add(change);
            }

            AccountStore.Update(_state.ToStorage());
        }
    }
}