using NatLibrary.Messages;

namespace NatMasterServer.Server {
    public interface IServerStorage<TKey> where TKey : notnull {
        Task<NatClientMessage> GetServerHost(TKey serverId);
        Task RegisterServer(TKey serverId, NatClientMessage message);
    }
}