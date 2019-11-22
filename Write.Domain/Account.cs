using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
        }

        public void PurchasePayPerView(Guid movieId)
        {
            EnsureExistingAccount();
            EnsureThatAnyCreditCardExists();
        }

        public void RemoveCreditCard(string creditCard)
        {
            EnsureExistingAccount();
            EnsureThatCreditCardExist(creditCard);

            Apply(new CreditCardRemovedEvent(creditCard));
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

    public class AccountState
    {
        public string Email { get; private set; }
        public string Username { get; private set; }
        public List<CreditCard> CreditCards { get; }

        public AccountState()
        {
            CreditCards = new List<CreditCard>();
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

        }

        private void When(CreditCardDemotedEvent @event)
        {

        }

        private void When(CreditCardRemovedEvent @event)
        {

        }

        public void Mutate(IAccountEvent e)
        {
            EventRedirector.ToWhen(this, e);
        }

        public class CreditCard
        {
            public string Number { get; }
            public bool IsPrimary { get; }

            public CreditCard(string number, bool isPrimary)
            {
                Number = number;
                IsPrimary = isPrimary;
            }
        }

    }

    public interface IAccountEvent
    {
        DateTimeOffset Timestamp { get; set; }
    }

    public class BecameCustomerEvent : IAccountEvent
    {
        public string Email { get; }
        public string Username { get; }

        public BecameCustomerEvent(string email, string username)
        {
            Email = email;
            Username = username;
            Timestamp = DateTimeOffset.Now;
        }

        public DateTimeOffset Timestamp { get; set; }
    }

    public class CreditCardAddedEvent : IAccountEvent
    {
        public string CreditCard { get; }
        public DateTimeOffset Timestamp { get; set; }

        public CreditCardAddedEvent(string creditCard)
        {
            CreditCard = creditCard;
            Timestamp = DateTimeOffset.Now;
        }
    }

    public class CreditCardDemotedEvent : IAccountEvent
    {
        public string CreditCard { get; }
        public DateTimeOffset Timestamp { get; set; }

        public CreditCardDemotedEvent(string creditCard)
        {
            CreditCard = creditCard;
            Timestamp = DateTimeOffset.Now;
        }
    }

    public class CreditCardPromotedEvent : IAccountEvent
    {
        public string CreditCard { get; }
        public DateTimeOffset Timestamp { get; set; }

        public CreditCardPromotedEvent(string creditCard)
        {
            CreditCard = creditCard;
            Timestamp = DateTimeOffset.Now;
        }
    }

    public class CreditCardRemovedEvent : IAccountEvent
    {
        public string CreditCard { get; }
        public DateTimeOffset Timestamp { get; set; }

        public CreditCardRemovedEvent(string creditCard)
        {
            CreditCard = creditCard;
            Timestamp = DateTimeOffset.Now;
        }
    }

    public static class EventRedirector
    {
        public static void ToWhen<T>(T instance, IAccountEvent e)
        {
            var method =
                instance.GetType()
                    .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance) //Public
                    .Where(x => x.Name == "When")
                    .SingleOrDefault(x => MethodSupportsEvent(e, x));

            if (method == null)
            {
                throw new InvalidOperationException($"Unable to locate event handler for {e.GetType().Name} in {instance.GetType().Name}");
            }

            method.Invoke(instance, new object[] { e });
        }

        private static bool MethodSupportsEvent(IAccountEvent e, MethodInfo method)
        {
            var parameters = method.GetParameters();
            var eventType = e.GetType();

            if (parameters.Length != 1)
            {
                return false;
            }

            var parameter = parameters.Single();

            return parameter.ParameterType == eventType;
        }
    }
}
