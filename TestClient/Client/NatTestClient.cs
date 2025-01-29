using NatLibrary.Messages;
using NatLibrary.Services;
using System.Net;

namespace TestClient.Client
{
    public class NatTestClient : IMessageHandler {
        private NatClient _client;
        private IPEndPoint _masterServerEndpoint;
        private IPEndPoint? _hostEndpoint;

        public NatTestClient()
        {
            _client = new NatClient(this);
            _masterServerEndpoint = new IPEndPoint(IPAddress.Loopback, 7000);
        }

        public Task SendData(NatClientMessage message) => _client.SendData(message, _masterServerEndpoint);

        public  Task Listen() => _client.Listen("Client");

        public Task HandleMessage(NatClient client, NatClientMessage message) {
            if (message.MessageType == MessageType.ClientConnectToHost) {

            } else if (message.MessageType == MessageType.Connected) {

            } else {
                Console.WriteLine("Invalid message");
            }

            Console.WriteLine(message);

            return Task.CompletedTask;
        }
    }
}
