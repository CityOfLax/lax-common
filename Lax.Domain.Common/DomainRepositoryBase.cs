﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lax.Domain.Common {

    public abstract class DomainRepositoryBase : IDomainRepository {

        public abstract Task<IEnumerable<IEvent>> Save<TAggregate>(TAggregate aggregate) where TAggregate : IAggregate;

        public abstract Task<TResult> GetById<TResult>(Guid id) where TResult : IAggregate, new();

        protected int CalculateExpectedVersion<T>(IAggregate aggregate, List<T> events) {
            var expectedVersion = aggregate.Version - events.Count;
            return expectedVersion;
        }

        protected TResult BuildAggregate<TResult>(IEnumerable<IEvent> events) where TResult : IAggregate, new() {
            var result = new TResult();
            foreach (var @event in events) {
                result.ApplyEvent(@event);
            }
            return result;
        }

    }

}