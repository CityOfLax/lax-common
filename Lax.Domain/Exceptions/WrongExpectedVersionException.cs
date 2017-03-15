using System;

namespace Lax.Domain.Exceptions {

    public class WrongExpectedVersionException : Exception {

        public WrongExpectedVersionException(string message)
            : base(message) { }

    }

}