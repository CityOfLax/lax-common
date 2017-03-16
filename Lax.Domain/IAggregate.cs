using System;
using System.Collections.Generic;

namespace Lax.Domain {

    public interface IAggregate<TAggregateState> where TAggregateState : class, new() {

        IEnumerable<IEvent> UncommitedEvents();

        void ClearUncommitedEvents();

        int Version { get; }

        Guid Id { get; }

        TAggregateState State { get; }

        void ApplyEvent(IEvent @event);

    }

}