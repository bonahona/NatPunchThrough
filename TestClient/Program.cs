using NatLibrary.Messages;
using System.Net;
using TestClient.Client;

namespace NatMasterServer
{
    internal class Program
    {
        private UdpClientWrapper _udpClient;
        private IPEndPoint _serverEndpoint;

        static void Main(string[] args)
        {
            new Program();
        }

        public Program()
        {
            _udpClient = new UdpClientWrapper();
            _serverEndpoint = new IPEndPoint(IPAddress.Loopback, 7000);

            Console.WriteLine("Start test server");

            var serverId = Random.Shared.Next(1000);
            while (true)
            {
                Console.WriteLine("Sending package....");
                var message = new NatClientMessage(MessageType.RegisterHost, serverId, _serverEndpoint);
                var sentCount = _udpClient.SendData(message);
                Console.WriteLine($"Sent {sentCount} bytes");

                Thread.Sleep(1000);
            }
        }
    }
}
