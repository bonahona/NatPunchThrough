using NatLibrary.Messages;
using NatLibrary.Services;
using System.Net;
using System.Net.Sockets;

namespace NatMasterServer.Server {
    public class MasterServer : IMessageHandler {
        private IPEndPoint _listenEndpoint;

        private NatClient _client;

        private ServerStorage<int> _storage = new ServerStorage<int>();

        public MasterServer() {
            _listenEndpoint = new IPEndPoint(IPAddress.Any, 7000);
            _client = new NatClient(this);
        }

        public async Task Run() {
            await _client.Listen("Master server");
        }

        private void SendData(NatClientMessage message, IPEndPoint recieverEndpoint) {
            var byteBuffer = message.GetBytes();
            Console.WriteLine(Convert.ToHexString(byteBuffer.ToArray()));
            var udpClient = new UdpClient();
            udpClient.Send(byteBuffer, byteBuffer.Length, recieverEndpoint);
        }

        public Task HandleMessage(NatClient client, NatClientMessage message) {
            if (message.MessageType == MessageType.RegisterHost) {
                return HandleHostMessage(message);
            } else if (message.MessageType == MessageType.RegisterClient) {
                return HandleClientMessage(message);
            } else {
                Console.WriteLine("Unknown message type");
                return Task.CompletedTask;
            }
        }

        private async Task HandleHostMessage(NatClientMessage message) {
            await _storage.RegisterServer(message.ServerId, message);
        }

        private async Task HandleClientMessage(NatClientMessage message) {
            var server = await _storage.GetServerHost(message.ServerId);

            Console.WriteLine($"Client {message.NatEndpoint} asking for server {message.ServerId}");

            var messageToHost = new NatClientMessage(MessageType.HostConnectToClient, server.ServerId, message.NatEndpoint);

            Console.WriteLine($"Sending Client info {message.NatEndpoint} to {server.NatEndpoint}");
            SendData(messageToHost, server.NatEndpoint);
        }
    }
}
