namespace Write.Commands
{
    public class BecomeCustomerCommand : ICommand
    {
        public string Username { get; }
        public string Email { get; set; }

        public BecomeCustomerCommand(string email, string username)
        {
            Email = email;
            Username = username;
        }
    }
}
