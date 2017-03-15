using System;
using Lax.Helpers.SoftlyDeletableEntity;
using Lax.Helpers.TimePeriods;

namespace Lax.Helpers.EffectiveDateEntity {

    public interface IEffectiveDateEntity<TEntity> : ISoftlyDeletableEntity<TEntity> where TEntity : class {

        ITimePeriod EffectiveTimePeriod { get; }

        void SetEffectiveEndDate(DateTime newEffectiveEndDate);

    }

}
