using System;

namespace Adventure.API.System
{
    [Serializable]
    public class InvalidOrEmptyException : Exception
    {
        public InvalidOrEmptyException() { }

        public InvalidOrEmptyException(string message)
            : base(message)
        {

        }
    }
}
