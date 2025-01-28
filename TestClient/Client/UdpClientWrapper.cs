using NatLibrary.Messages;
using System.Net;
using System.Net.Sockets;

namespace TestClient.Client
{
    public class UdpClientWrapper
    {
        private UdpClient _udpClient;
        private IPEndPoint _masterServerEndpoint;

        private IPEndPoint _hostEndpoint;

        public UdpClientWrapper()
        {
            _udpClient = new UdpClient();
            _masterServerEndpoint = new IPEndPoint(IPAddress.Loopback, 7000);
        }

        public async Task<int> SendData(NatClientMessage message)
        {
            var byteBuffer = message.GetBytes();
            return await _udpClient.SendAsync(byteBuffer, byteBuffer.Length, _masterServerEndpoint);
        }

        public async Task Listen() {
            await Console.Out.WriteLineAsync("Client avaiting endpoint...");

            while (_udpClient.Client.LocalEndPoint == null) {
                await Task.Delay(1000);
            }

            await Console.Out.WriteLineAsync("Start client listen");
            while (true) {
                try {
                    var result = await _udpClient.ReceiveAsync();
                    _hostEndpoint = result.RemoteEndPoint as IPEndPoint;

                    var message = new NatClientMessage(result.Buffer, result.RemoteEndPoint);
                    HandleMessage(message);
                } catch (Exception ex) {
                    await Console.Out.WriteLineAsync(ex.Message);
                }
            }
        }

        private void HandleMessage(NatClientMessage message) {
            if (message.MessageType == MessageType.ClientConnectToHost) {

            } else if (message.MessageType == MessageType.Connected) {

            } else {
                Console.WriteLine("Invalid message");
            }

            Console.WriteLine(message);
        }
    }
}
