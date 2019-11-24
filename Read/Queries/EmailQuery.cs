namespace Read.Queries
{
    public class EmailQuery : IQuery
    {
        public string Email { get; }

        public EmailQuery(string email)
        {
            Email = email;
        }
    }
}