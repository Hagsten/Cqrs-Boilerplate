namespace Write.Commands
{
    public class StartSubscriptionCommand : ICommand
    {
        public string Email { get; }

        public StartSubscriptionCommand(string email)
        {
            Email = email;
        }
    }
}