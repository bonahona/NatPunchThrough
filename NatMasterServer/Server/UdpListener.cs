using NatLibrary.Messages;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NatMasterServer.Server
{
    public class UdpListener
    {
        private const int RecieveBufferSize = 1024;

        private IPEndPoint _listenEndpoint;
        private Socket _socket;

        private byte[] _recieveBuffer = new byte[RecieveBufferSize];

        public UdpListener()
        {
            _listenEndpoint = new IPEndPoint(IPAddress.Any, 7000);
            _socket = new Socket(SocketType.Dgram, ProtocolType.Udp);
            _socket.ReceiveBufferSize = RecieveBufferSize;
            _socket.Bind(_listenEndpoint);
        }

        public void Listen()
        {
            Console.WriteLine("Start listening");
            while (true)
            {
                EndPoint senderEndpoint = new IPEndPoint(IPAddress.Any, 0);
                var recievedCount = _socket.ReceiveFrom(_recieveBuffer, ref senderEndpoint);
                Console.WriteLine(Convert.ToHexString(_recieveBuffer, 0, recievedCount));

                var message = new NatClientMessage(_recieveBuffer, (IPEndPoint)senderEndpoint);

                Console.WriteLine($"Recieved message: {message}");
            }
        }
    }
}
