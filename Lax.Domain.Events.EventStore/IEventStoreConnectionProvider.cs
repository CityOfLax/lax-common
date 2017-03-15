using System.Threading.Tasks;
using EventStore.ClientAPI;

namespace Lax.Domain.Events.EventStore {

    public interface IEventStoreConnectionProvider {

        Task<IEventStoreConnection> ProvideConnection();

    }

}