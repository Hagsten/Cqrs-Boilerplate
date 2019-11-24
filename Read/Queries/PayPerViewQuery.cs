using System;

namespace Read.Queries
{
    public class PayPerViewQuery : IQuery
    {
        public string Email { get; }
        public Guid MovieId { get; }

        public PayPerViewQuery(string email, Guid movieId)
        {
            Email = email;
            MovieId = movieId;
        }
    }
}