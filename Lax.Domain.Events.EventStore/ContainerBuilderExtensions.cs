using Autofac;

namespace Lax.Domain.Events.EventStore {

    public static class ContainerBuilderExtensions {

        public static void RegisterEventStoreDomainRepository(this ContainerBuilder builder, string connectionString) {

            builder.Register(c => new DefaultEventStoreConnectionProvider(connectionString))
                .As<IEventStoreConnectionProvider>()
                .InstancePerDependency();
            builder.RegisterType<EventStoreDomainRepository>().As<IDomainRepository>().InstancePerLifetimeScope();

        }

    }

}