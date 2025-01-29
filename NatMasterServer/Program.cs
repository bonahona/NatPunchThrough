using NatMasterServer.Server;

namespace NatMasterServer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await new Program().Run();
        }

        public async Task Run()
        {
            Console.WriteLine("Booting....");
            await new MasterServer().Run();
        }
    }
}
