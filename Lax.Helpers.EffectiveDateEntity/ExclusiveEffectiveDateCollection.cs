using System;
using System.Collections;
using System.Collections.Generic;
using Lax.Helpers.TimePeriods;
using System.Linq;

namespace Lax.Helpers.EffectiveDateEntity {


    /// <summary>
    /// Ensures that no two entities in the collection ever overlap each other.  When adding an object would cause an 
    /// overlap: the new objects time period overrides the old objects time periods, and the time periods of old objects are
    /// changed to suit.  (NOTE: Does not check condition of objects when collection is created.  Creator must ensure
    /// that no entities overlap each other.)
    /// </summary>
    /// <typeparam name="TEntity">The Entity to store in the collection.</typeparam>
    public class ExclusiveEffectiveDateCollection<TEntity> : IExclusiveEffectiveDateCollection<TEntity>, ICollection<TEntity> 
        where TEntity : class, IEffectiveDateEntity<TEntity> {

        private readonly List<TEntity> _values;

        public ExclusiveEffectiveDateCollection(IEnumerable<TEntity> values) {
            _values = new List<TEntity>(values);
        }

        public IEnumerator<TEntity> GetEnumerator() =>
            _values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        public void Add(TEntity item) {
            // 1. For all current entries contained in new entry, delete them.
            foreach (var entity in _values.Where(v => !v.IsDeleted && item.EffectiveTimePeriod.HasInside((ITimePeriod) v.EffectiveTimePeriod)).ToList()) {
                Remove(entity);
            }

            // 2. For all current entries after this entry, remove them.
            if (item.EffectiveTimePeriod.HasEnd) {
                foreach (var entity in _values.Where(v => !v.IsDeleted && 
                    (item.EffectiveTimePeriod.GetRelation(v.EffectiveTimePeriod) == PeriodRelation.Before ||
                     item.EffectiveTimePeriod.GetRelation(v.EffectiveTimePeriod) == PeriodRelation.EndTouching ||
                     item.EffectiveTimePeriod.GetRelation(v.EffectiveTimePeriod) == PeriodRelation.EndInside))) {

                    Remove(entity);
                }
            }

            // 3. Truncate all intersecting entities
            foreach (var entity in _values.Where(v => !v.IsDeleted && item.EffectiveTimePeriod.IntersectsWith(v.EffectiveTimePeriod))) {
                entity.SetEffectiveEndDate(item.EffectiveTimePeriod.Start.AddDays(-1));
            }

            // 4. Add the New Entity
            _values.Add(item);

        }

        public void Clear() {
            foreach (var entity in _values.Where(v => !v.IsDeleted)) {
                entity.Delete();
            }
        }

        public bool Contains(TEntity item) => _values.Contains(item);

        public void CopyTo(TEntity[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        public bool Remove(TEntity item) {
            if (_values.Contains(item) && !item.IsDeleted) {
                item.Delete();
                return true;
            }
            return false;
        }

        public int Count => _values.Count;

        public bool IsReadOnly => false;

        public TEntity this[DateTime effectiveDate] =>
            _values.FirstOrDefault(v => v.EffectiveTimePeriod.IntersectsWith(new TimeRange(effectiveDate.Date, true)) && !v.IsDeleted);

        public IEnumerable<TEntity> this[ITimePeriod effectiveTimePeriod] =>
            _values.Where(v => v.EffectiveTimePeriod.IntersectsWith(effectiveTimePeriod) && !v.IsDeleted);

    }

}
