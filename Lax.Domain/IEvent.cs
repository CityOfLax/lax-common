using System;

namespace Lax.Domain {

    public interface IEvent<TAggregateState> where TAggregateState : class, new() {

        Guid Id { get; }

    }

}