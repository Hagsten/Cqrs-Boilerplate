namespace Write.Commands
{
    public class RemoveCreditCardCommand : ICommand
    {
        public string Email { get; }
        public string Creditcard { get; }

        public RemoveCreditCardCommand(string email, string creditcard)
        {
            Email = email;
            Creditcard = creditcard;
        }
    }
}