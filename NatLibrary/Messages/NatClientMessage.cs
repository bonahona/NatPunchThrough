using System.IO;
using System.Net;
using System.Security.Cryptography;

namespace NatLibrary.Messages
{
    public class NatClientMessage
    {
        public MessageType MessageType { get; set; }
        public int ServerId { get; set; }
        public IPEndPoint ServerEndpoint { get; set; }
        public IPEndPoint NatEndpoint { get; set; }

        public NatClientMessage(MessageType type, int serverId, IPEndPoint serverEndpoint)
        {
            MessageType = type;
            ServerId = serverId;
            ServerEndpoint = serverEndpoint;
            NatEndpoint = new IPEndPoint(IPAddress.None, 0);
        }

        public NatClientMessage(byte[] buffer, IPEndPoint senderEndpoint)
        {
            var memoryStream = new MemoryStream(buffer);

            MessageType = (MessageType)memoryStream.ReadByte();

            ServerId = ReadInt(memoryStream);
            var ipAddress = ReadIpAddress(memoryStream);
            var port = ReadInt(memoryStream);

            ServerEndpoint = new IPEndPoint(ipAddress, port);

            NatEndpoint = senderEndpoint;
        }

        private Guid ReadGuid(Stream stream)
        {
            var guidBytes = new byte[16];
            stream.Read(guidBytes, 0, 16);
            return new Guid(guidBytes);
        }

        private IPAddress ReadIpAddress(Stream stream)
        {
            var ipAddressBytes = new byte[4];
            stream.Read(ipAddressBytes, 0, 4);
            return new IPAddress(ipAddressBytes);
        }

        private int ReadInt(Stream stream)
        {
            var buffer = new byte[4];
            stream.Read(buffer, 0, 4);
            return BitConverter.ToInt32(buffer);
        }

        public byte[] GetBytes()
        {
            var result = new MemoryStream();
            result.WriteByte((byte)MessageType);
            WriteInt(result, ServerId);
            WriteIPEndpoint(result, ServerEndpoint);
            WriteIPEndpoint(result, ServerEndpoint);

            return result.ToArray();
        }

        private void WriteInt(Stream stream, int value)
        {
            stream.Write(BitConverter.GetBytes(value));
        }

        private void WriteIPEndpoint(Stream stream, IPEndPoint endPoint) {
            stream.Write(endPoint.Address.GetAddressBytes());
            WriteInt(stream, endPoint.Port);
        }

        public override string ToString() => $"{MessageType} {ServerId} - {ServerEndpoint} - {NatEndpoint}";
    }
}
