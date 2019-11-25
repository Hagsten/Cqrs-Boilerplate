using System;
using System.Linq;
using Read;
using Write;
using Write.CommandHandlers;

namespace CqrsLunchAndLearn
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to this Lunch & Learn" + Environment.NewLine);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Available commands are: {Environment.NewLine}{string.Join(Environment.NewLine, GetTypes(typeof(ICommand)))}");
            Console.WriteLine(Environment.NewLine);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Available queries are: {Environment.NewLine}{string.Join(Environment.NewLine, GetTypes(typeof(IQuery)))}");

            Console.ForegroundColor = ConsoleColor.White;

            //RunMethodOne();
            RunMethodTwo();

            Console.ReadLine();
        }

        private static void RunMethodOne()
        {
            var runner = new RunnerOne(
                new StartSubscriptionCommandHandler(), 
                new AddCreditCardCommandHandler(), 
                new RemoveCreditCardCommandHandler(), 
                new PromoteCreditCardAsPrimaryCommandHandler(), 
                new BecomeCustomerCommandHandler(),
                new PurchasePayPerViewCommandHandler());

            runner.Run();
        }

        private static void RunMethodTwo()
        {
            var runner = new RunnerTwo(new CommandDispatcher());

            runner.Run();
        }

        public static string[] GetTypes(Type type)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && type != p);

            return types.Select(x => x.Name.Replace("Query", string.Empty).Replace("Command", string.Empty)).ToArray();
        }
    }
}
