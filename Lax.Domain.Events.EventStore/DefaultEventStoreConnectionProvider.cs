using System.Threading.Tasks;
using EventStore.ClientAPI;

namespace Lax.Domain.Events.EventStore {

    public class DefaultEventStoreConnectionProvider : IEventStoreConnectionProvider {

        private readonly string _connectionString;

        public DefaultEventStoreConnectionProvider(
            string connectionString) {

            _connectionString = connectionString;
        }

        public async Task<IEventStoreConnection> ProvideConnection() {
            var connection = EventStoreConnection.Create(_connectionString);
            await connection.ConnectAsync();
            return connection;
        }

    }

}