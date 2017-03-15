using System;

namespace Lax.Domain.Common {

    public class IdGenerator : IIdGenerator {

        public Guid GenerateGuid() => Guid.NewGuid();

    }

}