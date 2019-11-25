using System;
using System.Collections.Generic;
using System.Linq;
using Write;
using Write.CommandHandlers;

namespace CqrsLunchAndLearn
{
    public interface ICommandDispatcher
    {
        void Dispatch<TCommand>(TCommand command) where TCommand : ICommand;
    }

    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IEnumerable<Type> _commandHandlerTypes;

        public CommandDispatcher()
        {
            var type = typeof(ICommandHandler<>);

            _commandHandlerTypes = from x in AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                from z in x.GetInterfaces()
                let y = x.BaseType
                where
                    (y != null && y.IsGenericType &&
                     type.IsAssignableFrom(y.GetGenericTypeDefinition())) ||
                    (z.IsGenericType &&
                     type.IsAssignableFrom(z.GetGenericTypeDefinition()))
                select x;
        }

        public void Dispatch<TCommand>(TCommand command) where TCommand : ICommand
        {
            var type = _commandHandlerTypes.SingleOrDefault(x => x.GetInterfaces()[0].GenericTypeArguments[0] == typeof(TCommand));

            dynamic instance = Activator.CreateInstance(type);

            var loggingCommandHandler = new LoggingCommandHandler<TCommand>(instance);

            loggingCommandHandler.Handle(command);
        }
    }
}