namespace Lax.Helpers.SoftlyDeletableEntity {

    public interface ISoftlyDeletableEntity<TEntity> where TEntity : class {

        bool IsDeleted { get; }

        void Delete();

        void Restore();

    }

}
