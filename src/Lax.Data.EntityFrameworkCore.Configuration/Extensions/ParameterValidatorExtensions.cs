using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StaticDotNet.ParameterValidation;

namespace Lax.Data.EntityFrameworkCore.Configuration.Extensions {

    /// <summary>
    /// Adds additional parameter validation.
    /// </summary>
    public static class ParameterValidatorExtensions {

        /// <summary>
        /// Validates the parameter value is and <see cref="EntityTypeBuilder" /> for the <typeparamref name="TEntity" />. Otherwise an <see cref="ArgumentException" /> is thrown.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <param name="validator">The parameter validator.</param>
        /// <param name="entityTypeBuilder">The <see cref="EntityTypeBuilder{TEntity}" />.</param>
        /// <returns>The parameter validator.</returns>
        /// <exception cref="ArgumentException">Thrown when the parameter value is not an entity type builder for <typeparamref name="TEntity" />.</exception>
        public static ParameterValidator<EntityTypeBuilder> IsForEntityType<TEntity>(
            this ParameterValidator<EntityTypeBuilder> validator, out EntityTypeBuilder<TEntity> entityTypeBuilder)
            where TEntity : class {
            EntityTypeBuilder parameterValue = validator.Value;
            if (parameterValue != null && parameterValue.Metadata.ClrType != typeof(TEntity)) {
                string exceptionMessage = string.Format("Value must be entity type '{0}'.",
                    parameterValue.Metadata.ClrType.FullName);
                throw new ArgumentException(validator.Name, exceptionMessage);
            }

            InternalEntityTypeBuilder internalEntityTypeBuilder =
                ((IInfrastructure<InternalEntityTypeBuilder>) parameterValue).Instance;

            entityTypeBuilder = new EntityTypeBuilder<TEntity>(internalEntityTypeBuilder);

            return validator;
        }

    }

}