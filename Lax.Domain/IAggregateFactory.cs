using System;

namespace Lax.Domain {

    public interface IAggregateFactory<TAggregate, TAggregateState> where TAggregate : IAggregate<TAggregateState> where TAggregateState : class, new() {

        TAggregate CreateAggregate();

    }

}