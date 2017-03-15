using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lax.Domain {

    public interface IDomainRepository {

        Task<IEnumerable<IEvent>> Save<TAggregate>(TAggregate aggregate) where TAggregate : IAggregate;

        Task<TResult> GetById<TResult>(Guid id) where TResult : IAggregate, new();

    }

}