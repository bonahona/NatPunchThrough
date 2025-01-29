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
            _client = new NatClient(_listenEndpoint, this);
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

        public async Task HandleMessage(NatClient client, NatClientMessage message) {
            if (message.MessageType == MessageType.RegisterHost) {
                await HandleHostMessage(message);
            } else if (message.MessageType == MessageType.RegisterClient) {
                await HandleClientMessage(message);
            } else {
                await Console.Out.WriteLineAsync("Unknown message type");
            }
        }

        private async Task HandleHostMessage(NatClientMessage message) {
            await _storage.RegisterServer(message.ServerId, message);
        }

        private async Task HandleClientMessage(NatClientMessage message) {
            var server = await _storage.GetServerHost(message.ServerId);

            await Console.Out.WriteLineAsync($"Client {message.NatEndpoint} asking for server {message.ServerId}");

            var messageToHost = new NatClientMessage(MessageType.HostConnectToClient, server.ServerId, message.NatEndpoint);

            await Console.Out.WriteLineAsync($"Sending Client info {message.NatEndpoint} to {server.NatEndpoint}");
            SendData(messageToHost, server.NatEndpoint);
        }
    }
}
