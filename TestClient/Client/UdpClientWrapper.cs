using NatLibrary.Messages;
using System.Net;
using System.Net.Sockets;

namespace TestClient.Client
{
    public class UdpClientWrapper
    {
        private UdpClient _udpClient;
        private IPEndPoint _serverEndpoint;

        public UdpClientWrapper()
        {
            _udpClient = new UdpClient();
            _serverEndpoint = new IPEndPoint(IPAddress.Loopback, 7000);
        }

        public int SendData(NatClientMessage message)
        {
            var byteBuffer = message.GetBytes();
            Console.WriteLine(Convert.ToHexString(byteBuffer.ToArray()));
            return _udpClient.Send(byteBuffer, byteBuffer.Length, _serverEndpoint);
        }
    }
}
