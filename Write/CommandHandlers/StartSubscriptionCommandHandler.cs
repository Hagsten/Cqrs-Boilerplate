using Write.Commands;

namespace Write.CommandHandlers
{
    public class StartSubscriptionCommandHandler : ICommandHandler<StartSubscriptionCommand>
    {
        public void Handle(StartSubscriptionCommand command)
        {
            CommandProcess
                .Start(command.Email)
                .Execute(account => account.StartSubscription())
                .Complete();
        }
    }
}