using Write.Commands;

namespace Write.CommandHandlers
{
    public class RemoveCreditCardCommandHandler : ICommandHandler<RemoveCreditCardCommand>
    {
        public void Handle(RemoveCreditCardCommand command)
        {
            CommandProcess
                .Start(command.Email)
                .Execute(account => account.RemoveCreditCard(command.Creditcard))
                .Complete();
        }
    }
}