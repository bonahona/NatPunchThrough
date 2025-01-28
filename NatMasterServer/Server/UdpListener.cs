using NatLibrary.Messages;
using System.Net;
using System.Net.Sockets;

namespace NatMasterServer.Server {
    public class UdpListener {
        private const int RecieveBufferSize = 1024;

        private IPEndPoint _listenEndpoint;
        private Socket _socket;

        private ServerStorage<int> _storage = new ServerStorage<int>();

        private byte[] _recieveBuffer = new byte[RecieveBufferSize];

        public UdpListener() {
            _listenEndpoint = new IPEndPoint(IPAddress.Any, 7000);
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket.ReceiveBufferSize = RecieveBufferSize;
            _socket.Bind(_listenEndpoint);
        }

        public void Listen() {
            Console.WriteLine("Start listening");
            while (true) {
                try {
                    EndPoint senderEndpoint = new IPEndPoint(IPAddress.Any, 0);
                    var recievedCount = _socket.ReceiveFrom(_recieveBuffer, ref senderEndpoint);
                    var message = new NatClientMessage(_recieveBuffer, (IPEndPoint)senderEndpoint);
                    HandleMessage(message);
                }catch(Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void SendData(NatClientMessage message, IPEndPoint recieverEndpoint) {
            var byteBuffer = message.GetBytes();
            Console.WriteLine(Convert.ToHexString(byteBuffer.ToArray()));
            var udpClient = new UdpClient();
            udpClient.Send(byteBuffer, byteBuffer.Length, recieverEndpoint);
        }

        private void HandleMessage(NatClientMessage message) {
            if (message.MessageType == MessageType.RegisterHost) {
                HandleHostMessage(message);
            } else if (message.MessageType == MessageType.RegisterClient) {
                HandleClientMessage(message);
            } else {
                Console.WriteLine("Unknown message type");
            }
        }

        private void HandleHostMessage(NatClientMessage message) {
            _storage.RegisterServer(message.ServerId, message);
        }

        private void HandleClientMessage(NatClientMessage message) {
            var server = _storage.GetServerHost(message.ServerId);

            Console.WriteLine($"Client {message.NatEndpoint} asking for server {message.ServerId}");

            var messageToHost = new NatClientMessage(MessageType.HostConnectToClient, server.ServerId, message.NatEndpoint);

            Console.WriteLine($"Sending Client info {message.NatEndpoint} to {server.NatEndpoint}");
            SendData(messageToHost, server.NatEndpoint);
        }
    }
}
