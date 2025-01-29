using NatLibrary.Messages;
using System.Net;
using System.Net.Sockets;

namespace NatLibrary.Services
{
    public class NatClient
    {
        private const int BufferSize = 1024;

        private readonly Socket _socket;
        private readonly IMessageHandler _messageHandler;

        private readonly byte[] _recieveBuffer = new byte[BufferSize];

        public IPEndPoint LocalEndpoint() => (_socket.LocalEndPoint as IPEndPoint)!;

        public NatClient(IMessageHandler messageHandler)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket.ReceiveBufferSize = BufferSize;
            _messageHandler = messageHandler;
        }

        public NatClient(Socket socket, IMessageHandler messageHandler)
        {
            _socket = socket;
            _socket.ReceiveBufferSize = BufferSize;
            _messageHandler = messageHandler;
        }

        public NatClient(IPEndPoint localEndpoint, IMessageHandler messageHandler) {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket.Bind(localEndpoint);
            _socket.ReceiveBufferSize = BufferSize;
            _messageHandler = messageHandler;
        }

        public async Task Listen(string name) {
            await Console.Out.WriteLineAsync($"{name} avaiting endpoint...");

            while (_socket.LocalEndPoint == null) {
                await Task.Delay(1000);
            }

            await Console.Out.WriteLineAsync("Start client listen");
            while (true) {
                try {
                    await WaitForMessage();
                } catch (Exception ex) {
                    await Console.Out.WriteLineAsync(ex.Message);
                }
            }
        }

        public async Task<int> SendData(NatClientMessage message, IPEndPoint recieverEndpoint) {
            var byteBuffer = message.GetBytes();
            return await _socket.SendToAsync(byteBuffer, recieverEndpoint);
        }

        private async Task WaitForMessage() {
            EndPoint remoteEndpoint = new IPEndPoint(IPAddress.Any, 0);
            var result = await _socket.ReceiveFromAsync(_recieveBuffer, remoteEndpoint);

            var message = new NatClientMessage(_recieveBuffer, (result.RemoteEndPoint as IPEndPoint)!);
            await _messageHandler.HandleMessage(this, message);
        }
    }
}
