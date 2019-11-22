namespace Write.Commands
{
    public class AddCreditCardCommand : ICommand
    {
        public string Email { get; }
        public string CreditCard { get; }

        public AddCreditCardCommand(string email, string creditCard)
        {
            Email = email;
            CreditCard = creditCard;
        }
    }
}