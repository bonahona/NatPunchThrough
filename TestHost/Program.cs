using NatLibrary.Messages;
using System.Net;
using TestClient.Client;

namespace TestHost {
    internal class Program {
        private UdpClientWrapper _udpClient;
        private IPEndPoint _serverEndpoint;

        static async Task Main(string[] args) {
            await new Program().Run();
        }

        public Program() {
            _udpClient = new UdpClientWrapper();
            _serverEndpoint = new IPEndPoint(IPAddress.Loopback, 7000);
        }

        public async Task Run() {
            await Task.WhenAll(
                _udpClient.Listen(),
                SendDiscoverMessage()
            );
        }

        private async Task SendDiscoverMessage() {
            Console.WriteLine("Start test host");

            var serverId = Random.Shared.Next(1000);
            await Console.Out.WriteLineAsync($"Started host as ServerId {serverId}");
            while (true) {

                var message = new NatClientMessage(MessageType.RegisterHost, serverId, _serverEndpoint);
                var sentCount = await _udpClient.SendData(message);

                Console.WriteLine($"Host sent package({sentCount}) from {_udpClient.GetLocalEndpoint()}");
                Thread.Sleep(5000);
            }
        }
    }
}
