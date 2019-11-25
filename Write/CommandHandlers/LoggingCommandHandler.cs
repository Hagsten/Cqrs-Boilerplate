using System;

namespace Write.CommandHandlers
{
    public class LoggingCommandHandler<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
    {
        private readonly ICommandHandler<TCommand> _decoree;

        public LoggingCommandHandler(ICommandHandler<TCommand> decoree)
        {
            _decoree = decoree;
        }

        public void Handle(TCommand command)
        {
            Console.WriteLine("Before, yayayaya");

            _decoree.Handle(command);

            Console.WriteLine("After, yayayaya");
        }
    }
}