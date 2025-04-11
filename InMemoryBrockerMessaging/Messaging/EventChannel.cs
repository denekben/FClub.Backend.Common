using FClub.Backend.Common.InMemoryBrokerMessaging.Events;
using System.Threading.Channels;

namespace FClub.Backend.Common.InMemoryBrokerMessaging.Messaging
{
    internal sealed class EventChannel : IEventChannel
    {
        private readonly Channel<IEvent> _messages = Channel.CreateUnbounded<IEvent>();

        public ChannelReader<IEvent> Reader => _messages.Reader;
        public ChannelWriter<IEvent> Writer => _messages.Writer;
    }
}
