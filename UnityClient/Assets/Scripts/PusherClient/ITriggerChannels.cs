namespace PusherClient
{
    internal interface ITriggerChannels
    {
        IJsonSerializer Serializer { get; }
        void Trigger(string channelName, string eventName, object obj);

        void Unsubscribe(string channelName);
    }
}