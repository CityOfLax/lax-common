using System;
using System.Linq;
using System.Reflection;
using Autofac;

namespace Lax.Domain.Common {

    public static class ContainerBuilderExtensions {

        public static void RegisterDomain(this ContainerBuilder builder, params Assembly[] assemblies) {

            // Register IdGenerator
            builder.RegisterType<IdGenerator>().As<IIdGenerator>().InstancePerDependency();

            // Register IAggregate
            builder.RegisterTypesInAssembliesImplementingInterfaceAsInterface(typeof(IAggregate<>), assemblies);

            // Register ITransition
            builder.RegisterTypesInAssembliesImplementingInterfaceAsInterface(typeof(ITransition<,>), assemblies);

            // Register Generic Aggregate Factory
            builder.RegisterGeneric(typeof(AggregateFactory<,>))
                .As(typeof(IAggregateFactory<,>))
                .InstancePerDependency();

        }

        private static void RegisterTypesInAssembliesImplementingInterfaceAsInterface(
            this ContainerBuilder builder,
            Type type,
            params Assembly[] assemblies) {

            builder.RegisterTypes(
                    assemblies.SelectMany(assembly => assembly.GetTypes())
                        .Where(t => t.GetTypeInfo().IsAssignableFrom(type)).ToArray())
                .As(type)
                .InstancePerDependency();
        }

    }

}