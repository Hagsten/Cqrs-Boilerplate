namespace Write.Commands
{
    public class PromoteCreditCardAsPrimaryCommand : ICommand
    {
        public string Email { get; }
        public string CreditCard { get; }

        public PromoteCreditCardAsPrimaryCommand(string email, string creditCard)
        {
            Email = email;
            CreditCard = creditCard;
        }
    }
}