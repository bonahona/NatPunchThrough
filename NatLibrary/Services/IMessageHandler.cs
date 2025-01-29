using NatLibrary.Messages;

namespace NatLibrary.Services {
    public interface IMessageHandler {
        Task HandleMessage(NatClient client, NatClientMessage message);
    }
}
