using System;

namespace Lax.Domain.Exceptions {

    public class AggregateNotFoundException : Exception {

        public AggregateNotFoundException(string message)
            : base(message) { }

    }

}