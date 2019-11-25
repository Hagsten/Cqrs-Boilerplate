using Write.Commands;

namespace Write.CommandHandlers
{
    public class PurchasePayPerViewCommandHandler : ICommandHandler<PurchasePayPerViewCommand>
    {
        public void Handle(PurchasePayPerViewCommand command)
        {
            CommandProcess
                .Start(command.Email)
                .Execute(account => account.PurchasePayPerView(command.MovieId))
                .Complete();
        }
    }
}