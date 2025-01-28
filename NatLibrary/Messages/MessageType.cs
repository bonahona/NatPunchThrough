namespace NatLibrary.Messages
{
    public enum MessageType : byte
    {
        Invalid = 0,
        RegisterHost = 1,
        RegisterClient = 2,
        HostConnectToClient = 3,
        ClientConnectToHost = 4,
        Connected = 5
    }
}
