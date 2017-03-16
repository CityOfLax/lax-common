using System;
using System.Collections.Generic;
using Autofac;

namespace Lax.Domain.Common {

    public class AggregateBase<TAggregateState> : IAggregate<TAggregateState> where TAggregateState : class, new() {

        public int Version { get; protected set; } = -1;

        public Guid Id { get; protected set; }

        public TAggregateState State { get; protected set; } = new TAggregateState();

        private readonly List<IEvent<TAggregateState>> _uncommitedEvents = new List<IEvent<TAggregateState>>();

        private readonly IComponentContext _componentContext;

        protected AggregateBase(IComponentContext componentContext) {
            _componentContext = componentContext;
        }

        public void RaiseEvent(IEvent<TAggregateState> @event) {
            ApplyEvent(@event);
            _uncommitedEvents.Add(@event);
        }

        public void ApplyEvent(IEvent<TAggregateState> @event) {
            Id = @event.Id;
            var transition =
                (ITransition<TAggregateState, IEvent<TAggregateState>>)
                _componentContext.Resolve(typeof(ITransition<,>).MakeGenericType(typeof(TAggregateState),
                    typeof(IEvent<TAggregateState>)));
            State = transition.Apply(State, @event);
            Version++;
        }

        public IEnumerable<IEvent<TAggregateState>> UncommitedEvents() => _uncommitedEvents;

        public void ClearUncommitedEvents() {
            _uncommitedEvents.Clear();
        }

    }

}