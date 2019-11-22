using System;
using System.Linq;
using System.Reflection;
using Write;
using Write.CommandHandlers;
using Write.Commands;

namespace CqrsLunchAndLearn
{
    class Program
    {
        static void Main(string[] args)
        {
            var queries = new[]
            {
                "Q Test",
                "Q Test 2"
            };

            Console.WriteLine("Welcome to this Lunch & Learn" + Environment.NewLine);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Available commands are: {Environment.NewLine}{string.Join(Environment.NewLine, GetCommands())}");
            Console.WriteLine(Environment.NewLine);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Available queries are: {Environment.NewLine}{string.Join(Environment.NewLine, queries)}");

            Console.ForegroundColor = ConsoleColor.White;

            new BecomeCustomerCommandHandler().Handle(new BecomeCustomerCommand("foo.bar@gmail.com", "wozzaa"));
            new AddCreditCardCommandHandler().Handle(new AddCreditCardCommand("foo.bar@gmail.com", "9999-5555-3333-1111"));
            new PromoteCreditCardAsPrimaryCommandHandler().Handle(new PromoteCreditCardAsPrimaryCommand("foo.bar@gmail.com", "9999-5555-3333-1111"));

            Console.WriteLine(string.Join(Environment.NewLine, EventStore.GetHistory().Select(x => $"{x.Timestamp} : {x.GetType().Name.Replace("Event", string.Empty)}")));

            Console.ReadLine();
        }

        public static string[] GetCommands()
        {
            var type = typeof(ICommand);

            var commands = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && type != p);

            return commands.Select(x => x.Name.Replace("Command", string.Empty)).ToArray();
        }
    }
}
