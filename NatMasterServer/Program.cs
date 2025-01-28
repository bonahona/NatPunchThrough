using NatMasterServer.Server;

namespace NatMasterServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            new Program();
        }

        public Program()
        {
            Console.WriteLine("Booting....");
            new UdpListener().Listen();
        }
    }
}
