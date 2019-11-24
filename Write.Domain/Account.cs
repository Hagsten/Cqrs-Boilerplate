using System;
using System.Collections.Generic;
using System.Linq;
using Write.Domain.Events;

namespace Write.Domain
{
    public class Account
    {
        private readonly AccountState _state;
        private readonly List<IAccountEvent> _changes = new List<IAccountEvent>();

        private Account(AccountState state)
        {
            _state = state;
        }

        public static Account Create(AccountState state)
        {
            return new Account(state);
        }

        public IReadOnlyCollection<IAccountEvent> Changes()
        {
            return _changes.AsReadOnly();
        }

        private void Apply(IAccountEvent e)
        {
            _state.Mutate(e);
            _changes.Add(e);
        }

        public void BecomeCustomer(string email, string username)
        {
            EnsureNotExistingCustomer();

            Apply(new BecameCustomerEvent(email, username));
        }

        public void AddCreditCard(string creditCard)
        {
            EnsureExistingAccount();
            EnsureThatCreditCardDoesNotExist(creditCard);

            Apply(new CreditCardAddedEvent(creditCard));
        }

        public void PromoteCreditCard(string creditCard)
        {
            EnsureExistingAccount();
            EnsureThatCreditCardExist(creditCard);

            var primaryCard = _state.CreditCards.SingleOrDefault(x => x.IsPrimary);

            if (primaryCard != null)
            {
                Apply(new CreditCardDemotedEvent(primaryCard.Number));
            }

            Apply(new CreditCardPromotedEvent(creditCard));
        }

        public void StartSubscription()
        {
            EnsureExistingAccount();
            EnsureThatAnyCreditCardExists();

            Apply(new SubscriptionStartedEvent());
        }

        public void PurchasePayPerView(Guid movieId)
        {
            EnsureExistingAccount();
            EnsureThatAnyCreditCardExists();
            EnsureNotAlreadyPurchased(movieId);

            Apply(new PayPerViewPurchasedEvent(movieId));
        }

        public void RemoveCreditCard(string creditCard)
        {
            EnsureExistingAccount();
            EnsureThatCreditCardExist(creditCard);

            Apply(new CreditCardRemovedEvent(creditCard));
        }

        private void EnsureNotAlreadyPurchased(Guid movieId)
        {
            if (_state.ActivePayPerViews.Any(x => x == movieId))
            {
                throw new Exception("Movie already purchased");
            }
        }

        private void EnsureNotExistingCustomer()
        {
            if (!string.IsNullOrWhiteSpace(_state.Email))
            {
                throw new Exception($"{_state.Email} is already a customer");
            }
        }

        private void EnsureExistingAccount()
        {
            if (string.IsNullOrWhiteSpace(_state.Email))
            {
                throw new Exception("Can not do anything to a non-existing account");
            }
        }

        private void EnsureThatCreditCardDoesNotExist(string creditCard)
        {
            if (_state.CreditCards.Any(x => x.Number == creditCard))
            {
                throw new Exception($"Credit card {creditCard} already added");
            }
        }

        private void EnsureThatCreditCardExist(string creditCard)
        {
            if (_state.CreditCards.All(x => x.Number != creditCard))
            {
                throw new Exception($"Credit card {creditCard} does not exist");
            }
        }

        private void EnsureThatAnyCreditCardExists()
        {
            if (_state.CreditCards.Count == 0)
            {
                throw new Exception($"No credit card is connected to customer {_state.Email}");
            }
        }
    }
}
