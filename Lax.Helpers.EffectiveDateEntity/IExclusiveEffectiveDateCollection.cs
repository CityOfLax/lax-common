using System;
using System.Collections.Generic;
using Lax.Helpers.TimePeriods;

namespace Lax.Helpers.EffectiveDateEntity {

    public interface IExclusiveEffectiveDateCollection<TEntity> : ICollection<TEntity> where TEntity : class, IEffectiveDateEntity<TEntity> {

        TEntity this[DateTime effectiveDate] { get; }

        IEnumerable<TEntity> this[ITimePeriod effectiveTimePeriod] { get; }

    }

}
