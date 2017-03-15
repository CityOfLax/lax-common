using System;

namespace Lax.Domain {

    public interface IIdGenerator {

        Guid GenerateGuid();

    }

}