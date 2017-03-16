using System;

namespace Lax.Domain.Common {

    public class AggregateFactory<TAggregate, TAggregateState> : IAggregateFactory<TAggregate, TAggregateState> where TAggregateState : class, new() where TAggregate : IAggregate<TAggregateState> {

        private readonly Func<TAggregate> _aggregateCreationFunc;

        public AggregateFactory(
            Func<TAggregate> aggregateCreationFunc) {

            _aggregateCreationFunc = aggregateCreationFunc;
        }

        public TAggregate CreateAggregate() =>
            _aggregateCreationFunc();

    }

}
