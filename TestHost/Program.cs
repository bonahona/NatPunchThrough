using System.Net;
using TestClient.Client;

namespace TestHost {
    internal class Program {
        private TestClient.Client.NatTestHost _natHost;

        static async Task Main(string[] args) {
            await new Program().Run();
        }

        public Program() {
            _natHost = new TestClient.Client.NatTestHost();
        }

        public async Task Run() {
            await Task.WhenAll(
                _natHost.Listen(),
                _natHost.SendDiscoverMessage()
            );
        }       
    }
}
