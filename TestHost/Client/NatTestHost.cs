using NatLibrary.Messages;
using NatLibrary.Services;
using System.Net;


namespace TestClient.Client
{
    public class NatTestHost : IMessageHandler {
        private readonly NatClient _client;
        private IPEndPoint _masterServerEndpoint;

        private IPEndPoint? _clientEndpoint;

        public NatTestHost() {
            _client = new NatClient(this);
            _masterServerEndpoint = new IPEndPoint(IPAddress.Loopback, 7000);
        }

        public EndPoint GetLocalEndpoint() => _client.LocalEndpoint();

        public Task Listen() => _client.Listen("Host");

        public async Task SendDiscoverMessage() {
            Console.WriteLine("Start test host");

            var serverId = Random.Shared.Next(1000);
            await Console.Out.WriteLineAsync($"Started host as ServerId {serverId}");
            while (true) {

                var message = new NatClientMessage(MessageType.RegisterHost, serverId, _masterServerEndpoint);
                await _client.SendData(message, _masterServerEndpoint);

                Console.WriteLine($"Host sent package from {_client.LocalEndpoint()}");
                Thread.Sleep(5000);
            }
        }

        public async Task HandleMessage(NatClient client, NatClientMessage message) {
            if (message.MessageType == MessageType.HostConnectToClient) {
                await HandleConnectToClientMessage(message);
            } else if (message.MessageType == MessageType.Connected) {

            } else {
                Console.WriteLine("Invalid message");
            }

            Console.WriteLine(message);
        }

        private async Task HandleConnectToClientMessage(NatClientMessage message) {
            await SendData(message);
        }

        public Task SendData(NatClientMessage message) => _client.SendData(message, _masterServerEndpoint);
    }
}
