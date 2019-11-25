using System;
using System.Collections.Generic;
using System.Linq;
using Write.Domain.Events;

namespace Write.Domain
{
    public class AccountState
    {
        public string Email { get; private set; }
        public string Username { get; private set; }
        public List<Guid> ActivePayPerViews { get; private set; }
        public List<CreditCard> CreditCards { get; private set; }
        public Subscription ActiveSubscription { get; private set; }






















        public AccountState()
        {
            CreditCards = new List<CreditCard>();
            ActivePayPerViews = new List<Guid>();
        }

        public static AccountState FromStorage(Persistance.Account account)
        {
            if (account == null)
            {
                return null;
            }

            return new AccountState
            {
                Email = account.Email,
                Username = account.Username,
                CreditCards = account.CreditCards.Select(x => new CreditCard(x.Number, x.IsPrimary)).ToList(),
                ActiveSubscription = account.ActiveSubscription != null ? new Subscription(account.ActiveSubscription.ActiveFrom, account.ActiveSubscription.ActiveTo) : null,
                ActivePayPerViews = account.PayPerViews
            };
        }

        public Persistance.Account ToStorage()
        {
            return new Persistance.Account
            {
                Email = Email,
                Username = Username,
                PayPerViews = ActivePayPerViews,
                CreditCards = CreditCards.Select(x => new Persistance.Account.CreditCard
                {
                    IsPrimary = x.IsPrimary,
                    Number = x.Number
                }).ToList(),
                ActiveSubscription = ActiveSubscription != null ? new Persistance.Account.Subscription
                {
                    ActiveFrom = ActiveSubscription.ActiveFrom,
                    ActiveTo = ActiveSubscription.ActiveTo
                } : null
            };
        }

        private void When(BecameCustomerEvent @event)
        {
            Email = @event.Email;
            Username = @event.Username;
        }

        private void When(CreditCardAddedEvent @event)
        {
            CreditCards.Add(new CreditCard(@event.CreditCard, false));
        }

        private void When(CreditCardPromotedEvent @event)
        {
            var card = CreditCards.Single(x => x.Number == @event.CreditCard);

            card.SetAsPrimary();
        }

        private void When(CreditCardDemotedEvent @event)
        {
            var card = CreditCards.Single(x => x.Number == @event.CreditCard);

            card.SetAsNotPrimary();
        }

        private void When(CreditCardRemovedEvent @event)
        {
            CreditCards = CreditCards.Where(x => x.Number != @event.CreditCard).ToList();
        }

        private void When(SubscriptionStartedEvent @event)
        {
            ActiveSubscription = new Subscription(DateTimeOffset.Now, DateTimeOffset.Now.AddDays(10));
        }

        private void When(PayPerViewPurchasedEvent @event)
        {
            ActivePayPerViews.Add(@event.MovieId);
        }

        public void Mutate(IAccountEvent e)
        {
            EventRedirector.ToWhen(this, e);
        }

        public class CreditCard
        {
            public string Number { get; }
            public bool IsPrimary { get; private set; }

            public CreditCard(string number, bool isPrimary)
            {
                Number = number;
                IsPrimary = isPrimary;
            }

            public void SetAsPrimary()
            {
                IsPrimary = true;
            }

            public void SetAsNotPrimary()
            {
                IsPrimary = false;
            }
        }

        public class Subscription
        {
            public DateTimeOffset ActiveFrom { get; }
            public DateTimeOffset ActiveTo { get; }

            public Subscription(DateTimeOffset activeFrom, DateTimeOffset activeTo)
            {
                ActiveTo = activeTo;
                ActiveFrom = activeFrom;
            }
        }
    }
}