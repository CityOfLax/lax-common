using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lax.Domain.Common {

    public abstract class DomainRepositoryBase : IDomainRepository {
        

        public abstract Task<TAggregate> GetById<TAggregate, TAggregateState>(Guid id) where TAggregate : IAggregate<TAggregateState>, new() where TAggregateState : class, new();

        protected int CalculateExpectedVersion<T, TAggregateState>(IAggregate<TAggregateState> aggregate, List<T> events) where TAggregateState : class, new() {
            var expectedVersion = aggregate.Version - events.Count;
            return expectedVersion;
        }

        protected TResult BuildAggregate<TResult, TAggregateState>(IEnumerable<IEvent<TAggregateState>> events) where TResult : IAggregate<TAggregateState>, new() where TAggregateState : class, new() {
            var result = new TResult();
            foreach (var @event in events) {
                result.ApplyEvent(@event);
            }
            return result;
        }

        public abstract Task<IEnumerable<IEvent<TAggregateState>>> Save<TAggregate, TAggregateState>(TAggregate aggregate)
            where TAggregate : IAggregate<TAggregateState> where TAggregateState : class, new();

    }

}