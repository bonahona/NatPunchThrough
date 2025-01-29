using NatLibrary.Messages;

namespace NatMasterServer.Server {
    public class ServerStorage<TKey> : IServerStorage<TKey> where TKey : notnull {
        private Dictionary<TKey, NatClientMessage> _serverHostMapping { get; set; } = new Dictionary<TKey, NatClientMessage>();

        public Task RegisterServer(TKey serverId, NatClientMessage message) {
            if (_serverHostMapping.ContainsKey(serverId)) {
                _serverHostMapping[serverId] = message;
            } else {
                Console.WriteLine($"Registered new host {serverId} as {message.NatEndpoint}");
                _serverHostMapping.Add(serverId, message);
            }

            return Task.CompletedTask;
        }

        public Task<NatClientMessage> GetServerHost(TKey serverId) {
            if (!_serverHostMapping.ContainsKey(serverId)) {
                throw new Exception("Id not found");
            }

            return Task.FromResult(_serverHostMapping[serverId]);
        }
    }
}
