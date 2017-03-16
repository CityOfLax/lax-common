using System;
using System.Collections.Generic;

namespace Lax.Domain.Common {

    public class AggregateBase<TAggregateState> : IAggregate where TAggregateState : class, new() {

        public int Version { get; protected set; } = -1;

        public Guid Id { get; protected set; }

        public TAggregateState State { get; protected set; } = new TAggregateState();

        private readonly List<IEvent> _uncommitedEvents = new List<IEvent>();

        private readonly Dictionary<Type, AggregateTransitionRouteEntry<TAggregateState>> _transitionRoutes = new Dictionary<Type,AggregateTransitionRouteEntry<TAggregateState>>();

        public void RaiseEvent(IEvent @event) {
            ApplyEvent(@event);
            _uncommitedEvents.Add(@event);
        }

        protected void RegisterTransition<TEvent>(
            Func<TAggregateState, IEvent, TAggregateState> transitionFunc, 
            bool isCreateTransition = false) where TEvent : IEvent {
            _transitionRoutes.Add(
                typeof(TEvent), 
                new AggregateTransitionRouteEntry<TAggregateState>(
                    transitionFunc,
                    isCreateTransition));
        }

        public void ApplyEvent(IEvent @event) {
            var eventType = @event.GetType();
            if (_transitionRoutes.ContainsKey(eventType)) {
                var routeEntry = _transitionRoutes[eventType];
                if (routeEntry.IsCreateTransition) {
                    Id = @event.Id;
                }
                State = routeEntry.ApplyTransitionRouteEntry(State, @event);
            }
            Version++;
        }

        public IEnumerable<IEvent> UncommitedEvents() => _uncommitedEvents;

        public void ClearUncommitedEvents() {
            _uncommitedEvents.Clear();
        }

    }

}