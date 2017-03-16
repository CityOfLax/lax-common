using System;

namespace Lax.Domain.Common {

    internal class AggregateTransitionRouteEntry<TAggregateState> {

        public bool IsCreateTransition { get; }

        public Func<TAggregateState, IEvent, TAggregateState> TransitionFunc { get; }

        public AggregateTransitionRouteEntry(
            Func<TAggregateState, IEvent, TAggregateState> transitionFunc,
            bool isCreateTransition) {

            TransitionFunc = transitionFunc;
            IsCreateTransition = isCreateTransition;
        }

        public TAggregateState ApplyTransitionRouteEntry<TEvent>(
            TAggregateState currentState,
            TEvent @event) where TEvent : IEvent => TransitionFunc(currentState, @event);

    }

}