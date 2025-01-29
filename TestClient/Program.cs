using NatLibrary.Messages;
using System.Net;
using TestClient.Client;

namespace TestClient {
    internal class Program {
        private NatTestClient _client;
        private IPEndPoint _masterServerEndpoint;

        static async Task Main(string[] args) {
            await new Program().Run();
        }

        public Program() {
            _client = new NatTestClient();
            _masterServerEndpoint = new IPEndPoint(IPAddress.Loopback, 7000);
        }

        public async Task Run() { 
            Console.WriteLine("Start test client");
            await Task.WhenAll(
                _client.Listen(),
                ClientManualInput()
            );
        }

        private async Task ClientManualInput() {
            while (true) {
                try {
                    Console.WriteLine("Enter server id:");
                    var serverId = int.Parse(Console.ReadLine()!);
                    Console.WriteLine("Sending package....");
                    var message = new NatClientMessage(MessageType.RegisterClient, serverId, _masterServerEndpoint);
                    await _client.SendData(message);

                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
