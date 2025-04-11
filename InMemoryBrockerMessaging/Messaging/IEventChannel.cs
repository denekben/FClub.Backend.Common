using FClub.Backend.Common.InMemoryBrokerMessaging.Events;
using System.Threading.Channels;

namespace FClub.Backend.Common.InMemoryBrokerMessaging.Messaging
{
    internal interface IEventChannel
    {
        ChannelReader<IEvent> Reader { get; }
        ChannelWriter<IEvent> Writer { get; }
    }
}
