using System;

namespace Lax.Domain.Common {

    public class AggregateFactory<TAggregate, TAggregateState, TEvent> : IAggregateFactory<TAggregate, TAggregateState, TEvent> where TEvent : IEvent where TAggregateState : class, new() where TAggregate : IAggregate<TAggregateState> {

        private readonly Func<TEvent, TAggregate> _aggregateCreationFunc;

        public AggregateFactory(
            Func<TEvent, TAggregate> aggregateCreationFunc) {

            _aggregateCreationFunc = aggregateCreationFunc;
        }

        public TAggregate CreateAggregate(TEvent createEvent) =>
            _aggregateCreationFunc(createEvent);

    }

}
