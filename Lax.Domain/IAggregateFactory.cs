using System;

namespace Lax.Domain {

    public interface IAggregateFactory<TAggregate, TAggregateState, TEvent> where TAggregate : IAggregate<TAggregateState> where TEvent : IEvent<TAggregateState> where TAggregateState : class, new() {

        TAggregate CreateAggregate(TEvent createEvent);

    }

}