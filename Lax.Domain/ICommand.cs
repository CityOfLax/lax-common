using System;
using MediatR;

namespace Lax.Domain {

    public interface ICommand : IRequest<Guid> {

    }

}
