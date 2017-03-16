using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;

namespace Lax.Domain.Common {

    public abstract class DomainRepositoryBase : IDomainRepository {

        private readonly IComponentContext _componentContext;

        protected DomainRepositoryBase(
            IComponentContext componentContext) {

            _componentContext = componentContext;
        }

        public abstract Task<TAggregate> GetById<TAggregate, TAggregateState>(Guid id) where TAggregate : IAggregate<TAggregateState> where TAggregateState : class, new();

        protected int CalculateExpectedVersion<T, TAggregateState>(IAggregate<TAggregateState> aggregate, List<T> events) where TAggregateState : class, new() {
            var expectedVersion = aggregate.Version - events.Count;
            return expectedVersion;
        }

        protected TResult BuildAggregate<TResult, TAggregateState>(IEnumerable<IEvent> events) where TResult : IAggregate<TAggregateState> where TAggregateState : class, new() {
            var result = _componentContext.Resolve<TResult>();
            foreach (var @event in events) {
                result.ApplyEvent(@event);
            }
            return result;
        }

        public abstract Task<IEnumerable<IEvent>> Save<TAggregate, TAggregateState>(TAggregate aggregate)
            where TAggregate : IAggregate<TAggregateState> where TAggregateState : class, new();

    }

}