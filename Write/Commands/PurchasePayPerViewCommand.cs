using System;

namespace Write.Commands
{
    public class PurchasePayPerViewCommand : ICommand
    {
        public string Email { get; }
        public Guid MovieId { get; }

        public PurchasePayPerViewCommand(string email, Guid movieId)
        {
            Email = email;
            MovieId = movieId;
        }
    }
}