using NatLibrary.Messages;

namespace NatMasterServer.Server
{
    public class ServerStorage<TKey> where TKey : notnull
    {
        private Dictionary<TKey, NatClientMessage> _serverHostMapping { get; set; } = new Dictionary<TKey, NatClientMessage>();

        public void RegisterServer(TKey serverId, NatClientMessage message)
        {
            if(_serverHostMapping.ContainsKey(serverId))
            {
                _serverHostMapping[serverId] = message;
            } else
            {
                _serverHostMapping.Add(serverId, message);
            }
        }

        public NatClientMessage GetServerHost(TKey serverId)
        {
            if(!_serverHostMapping.ContainsKey(serverId))
            {
                throw new Exception("Id not found");
            }

            return _serverHostMapping[serverId];
        }
    }
}
