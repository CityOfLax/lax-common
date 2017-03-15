using System;
using MediatR;

namespace Lax.Domain {

    public interface ICommandHandler<TCommand> : IAsyncRequestHandler<TCommand, Guid> where TCommand : ICommand {
        
    }

}