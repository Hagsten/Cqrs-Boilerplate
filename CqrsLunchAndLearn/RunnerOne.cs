using System;
using System.Linq;
using Read;
using Read.Queries;
using Read.QueryHandlers;
using Write;
using Write.Commands;

namespace CqrsLunchAndLearn
{
    public class RunnerOne
    {
        private readonly ICommandHandler<StartSubscriptionCommand> _startSubscriptionCommandHandler;
        private readonly ICommandHandler<AddCreditCardCommand> _addCreditcardCommandHandler;
        private readonly ICommandHandler<RemoveCreditCardCommand> _removeCreditCardCommandHandler;
        private readonly ICommandHandler<PromoteCreditCardAsPrimaryCommand> _promoteCreditCardCommandHandler;
        private readonly ICommandHandler<BecomeCustomerCommand> _becomeCustomerCommandHandler;
        private readonly ICommandHandler<PurchasePayPerViewCommand> _purchasePpwCommandHandler;

        public RunnerOne(
            ICommandHandler<StartSubscriptionCommand> startSubscriptionCommandHandler,
            ICommandHandler<AddCreditCardCommand> addCreditcardCommandHandler,
            ICommandHandler<RemoveCreditCardCommand> removeCreditCardCommandHandler,
            ICommandHandler<PromoteCreditCardAsPrimaryCommand> promoteCreditCardCommandHandler,
            ICommandHandler<BecomeCustomerCommand> becomeCustomerCommandHandler,
            ICommandHandler<PurchasePayPerViewCommand> purchasePpwCommandHandler)
        {
            _startSubscriptionCommandHandler = startSubscriptionCommandHandler;
            _addCreditcardCommandHandler = addCreditcardCommandHandler;
            _removeCreditCardCommandHandler = removeCreditCardCommandHandler;
            _promoteCreditCardCommandHandler = promoteCreditCardCommandHandler;
            _becomeCustomerCommandHandler = becomeCustomerCommandHandler;
            _purchasePpwCommandHandler = purchasePpwCommandHandler;
        }

        public void Run()
        {
            var movie = Guid.NewGuid();
            var emailQuery = new EmailQuery("foo.bar@gmail.com");

            Console.WriteLine($"Is customer: {new IsCustomerQueryHandler().Handle(emailQuery)}");

            Console.WriteLine("Press enter to become customer");
            Run(() => _becomeCustomerCommandHandler.Handle(new BecomeCustomerCommand("foo.bar@gmail.com", "wozzaa")));

            Console.WriteLine($"Is customer: {new IsCustomerQueryHandler().Handle(emailQuery)}");
            Console.WriteLine($"Has subscription: {new HasSubscriptionQueryHandler().Handle(emailQuery)}");

            Console.WriteLine("Press enter to start subscription");
            Run(() => _startSubscriptionCommandHandler.Handle(new StartSubscriptionCommand("foo.bar@gmail.com")));

            Console.WriteLine(
                $"Can view movie: {new CanViewQueryHandler().Handle(new PayPerViewQuery("foo.bar@gmail.com", movie))}");

            Console.WriteLine("Press enter to purchase PPW");
            Run(() => _purchasePpwCommandHandler.Handle(new PurchasePayPerViewCommand("foo.bar@gmail.com", movie)));

            Console.WriteLine("Press enter to add credit card");
            Run(() => _addCreditcardCommandHandler.Handle(new AddCreditCardCommand("foo.bar@gmail.com",
                "9999-5555-3333-1111")));

            Console.WriteLine("Press enter to promote credit card");
            Run(() => _promoteCreditCardCommandHandler.Handle(
                new PromoteCreditCardAsPrimaryCommand("foo.bar@gmail.com", "9999-5555-3333-1111")));

            Console.WriteLine("Press enter to start subscription");
            Run(() => _startSubscriptionCommandHandler.Handle(new StartSubscriptionCommand("foo.bar@gmail.com")));

            Console.WriteLine($"Has subscription: {new HasSubscriptionQueryHandler().Handle(emailQuery)}");

            Console.WriteLine("Press enter to purchase PPW");
            Run(() => _purchasePpwCommandHandler.Handle(new PurchasePayPerViewCommand("foo.bar@gmail.com", movie)));

            Console.WriteLine(
                $"Can view movie: {new CanViewQueryHandler().Handle(new PayPerViewQuery("foo.bar@gmail.com", movie))}");

            Console.WriteLine("Press enter to add another credit card");
            Run(() => _addCreditcardCommandHandler.Handle(new AddCreditCardCommand("foo.bar@gmail.com",
                "1111-2222-3333-1111")));

            Console.WriteLine("Press enter to promote credit card");
            Run(() => _promoteCreditCardCommandHandler.Handle(
                new PromoteCreditCardAsPrimaryCommand("foo.bar@gmail.com", "1111-2222-3333-1111")));

            Console.WriteLine("Press enter to remove credit card");
            Run(() => _removeCreditCardCommandHandler.Handle(new RemoveCreditCardCommand("foo.bar@gmail.com",
                "9999-5555-3333-1111")));

            Console.WriteLine("Press enter to see event stack");
            Run(() => Console.WriteLine(string.Join(Environment.NewLine,
                EventStore.GetHistory().Select(x => $"{x.Timestamp} : {x.GetType().Name.Replace("Event", string.Empty)}"))));
        }

        private static void Run(Action a)
        {
            try
            {
                Console.ReadLine();
                a();
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}