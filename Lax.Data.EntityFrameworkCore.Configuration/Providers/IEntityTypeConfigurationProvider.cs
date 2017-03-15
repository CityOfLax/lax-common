using System.Collections.Generic;

namespace Lax.Data.EntityFrameworkCore.Configuration.Providers {

    /// <summary>
    /// Responsible for getting a collection of <see cref="IEntityTypeConfiguration" />.
    /// </summary>
    public interface IEntityTypeConfigurationProvider {

        /// <summary>
        /// Returns a collection of <see cref="IEntityTypeConfiguration" />.
        /// </summary>
        /// <returns>A collection of <see cref="IEntityTypeConfiguration" />.</returns>
        IEnumerable<IEntityTypeConfiguration> GetConfigurations();

    }

}