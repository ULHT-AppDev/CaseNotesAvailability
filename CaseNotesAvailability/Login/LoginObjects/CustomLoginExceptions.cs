using System;

namespace Login.LoginObjects
{
    public class CreateSessionIDException : Exception
    {
        public CreateSessionIDException()
        {
        }

        public CreateSessionIDException(string message) : base(message)
        {
        }

        public CreateSessionIDException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}