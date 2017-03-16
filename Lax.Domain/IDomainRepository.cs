using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lax.Domain {

    public interface IDomainRepository {

        Task<IEnumerable<IEvent<TAggregateState>>> Save<TAggregate, TAggregateState>(TAggregate aggregate) where TAggregate : IAggregate<TAggregateState> where TAggregateState : class, new();

        Task<TAggregate> GetById<TAggregate, TAggregateState>(Guid id) where TAggregate : IAggregate<TAggregateState>, new() where TAggregateState : class, new();

    }

}