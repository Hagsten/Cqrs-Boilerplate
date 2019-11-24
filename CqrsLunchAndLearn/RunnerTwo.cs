using System;
using System.Linq;
using Read;
using Read.Queries;
using Read.QueryHandlers;
using Write;
using Write.Commands;

namespace CqrsLunchAndLearn
{
    public class RunnerTwo
    {
        private readonly ICommandDispatcher _dispatcher;

        public RunnerTwo(ICommandDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public void Run()
        {
            var movie = Guid.NewGuid();
            var emailQuery = new EmailQuery("foo.bar@gmail.com");

            Console.WriteLine($"Is customer: {new IsCustomerQueryHandler().Handle(emailQuery)}");

            Console.WriteLine("Press enter to become customer");
            Run(() => _dispatcher.Dispatch(new BecomeCustomerCommand("foo.bar@gmail.com", "wozzaa")));

            Console.WriteLine($"Is customer: {new IsCustomerQueryHandler().Handle(emailQuery)}");
            Console.WriteLine($"Has subscription: {new HasSubscriptionQueryHandler().Handle(emailQuery)}");

            Console.WriteLine("Press enter to start subscription");
            Run(() => _dispatcher.Dispatch(new StartSubscriptionCommand("foo.bar@gmail.com")));

            Console.WriteLine($"Can view movie: {new CanViewQueryHandler().Handle(new PayPerViewQuery("foo.bar@gmail.com", movie))}");

            Console.WriteLine("Press enter to purchase PPW");
            Run(() => _dispatcher.Dispatch(new PurchasePayPerViewCommand("foo.bar@gmail.com", movie)));

            Console.WriteLine("Press enter to add credit card");
            Run(() => _dispatcher.Dispatch(new AddCreditCardCommand("foo.bar@gmail.com",
                "9999-5555-3333-1111")));

            Console.WriteLine("Press enter to promote credit card");
            Run(() => _dispatcher.Dispatch(new PromoteCreditCardAsPrimaryCommand("foo.bar@gmail.com", "9999-5555-3333-1111")));

            Console.WriteLine("Press enter to start subscription");
            Run(() => _dispatcher.Dispatch(new StartSubscriptionCommand("foo.bar@gmail.com")));

            Console.WriteLine($"Has subscription: {new HasSubscriptionQueryHandler().Handle(emailQuery)}");

            Console.WriteLine("Press enter to purchase PPW");
            Run(() => _dispatcher.Dispatch(new PurchasePayPerViewCommand("foo.bar@gmail.com", movie)));

            Console.WriteLine($"Can view movie: {new CanViewQueryHandler().Handle(new PayPerViewQuery("foo.bar@gmail.com", movie))}");

            Console.WriteLine("Press enter to add another credit card");
            Run(() => _dispatcher.Dispatch(new AddCreditCardCommand("foo.bar@gmail.com",
                "1111-2222-3333-1111")));

            Console.WriteLine("Press enter to promote credit card");
            Run(() => _dispatcher.Dispatch(new PromoteCreditCardAsPrimaryCommand("foo.bar@gmail.com", "1111-2222-3333-1111")));

            Console.WriteLine("Press enter to remove credit card");
            Run(() => _dispatcher.Dispatch(new RemoveCreditCardCommand("foo.bar@gmail.com",
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