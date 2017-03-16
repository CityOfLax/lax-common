namespace Lax.Domain {

    public interface ITransition<TAggregateState, TEvent> where TAggregateState : class, new() where TEvent : IEvent {

        TAggregateState Apply(
            TAggregateState currentState,
            TEvent @event);

    }

}