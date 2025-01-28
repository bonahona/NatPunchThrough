using NatLibrary.Messages;
using System.Net;
using System.Net.Sockets;

namespace TestClient.Client
{
    public class UdpClientWrapper
    {
        private UdpClient _udpClient;
        private IPEndPoint _masterServerEndpoint;

        private IPEndPoint _clientEndpoint;

        public UdpClientWrapper()
        {
            _udpClient = new UdpClient();
            _masterServerEndpoint = new IPEndPoint(IPAddress.Loopback, 7000);
        }

        public EndPoint GetLocalEndpoint() => _udpClient.Client.LocalEndPoint!;

        public async Task Listen() {
            await Console.Out.WriteLineAsync("Host avaiting endpoint...");

            while(_udpClient.Client.LocalEndPoint == null) {
                await Task.Delay(1000);
            }

            await Console.Out.WriteLineAsync("Start host listen");
            while (true) {
                try {
                    var result = await _udpClient.ReceiveAsync();
                    _clientEndpoint = result.RemoteEndPoint as IPEndPoint;

                    var message = new NatClientMessage(result.Buffer, result.RemoteEndPoint);
                    await HandleMessage(message);
                } catch (Exception ex) {
                    await Console.Out.WriteLineAsync(ex.Message);
                }
            }
        }

        private async Task HandleMessage(NatClientMessage message) {
            if(message.MessageType == MessageType.HostConnectToClient) {
                await HandleConnectToClientMessage(message);
            } else if(message.MessageType == MessageType.Connected) {

            } else {
                Console.WriteLine("Invalid message");
            }

            Console.WriteLine(message);
        }

        private async Task HandleConnectToClientMessage(NatClientMessage message) {
            await SendData(message);
        }

        public async Task<int> SendData(NatClientMessage message)
        {
            var byteBuffer = message.GetBytes();
            return await _udpClient.SendAsync(byteBuffer, byteBuffer.Length, _masterServerEndpoint);
        }
    }
}
