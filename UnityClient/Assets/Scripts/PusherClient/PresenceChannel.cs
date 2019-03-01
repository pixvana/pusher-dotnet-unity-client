using System.Collections.Generic;

namespace PusherClient
{
    /// <summary>
    /// Represents a Pusher Presence Channel that can be subscribed to
    /// </summary>
    public class PresenceChannel : PrivateChannel
    {
        /// <summary>
        /// Fires when a Member is Added
        /// </summary>
        public event MemberAddedEventHandler MemberAdded;

        /// <summary>
        /// Fires when a Member is Removed
        /// </summary>
        public event MemberRemovedEventHandler MemberRemoved;

        internal PresenceChannel(string channelName, ITriggerChannels pusher) : base(channelName, pusher) { }

        /// <summary>
        /// Gets the Members of the channel
        /// </summary>
        public Dictionary<string, object> Members { get; private set; } = new Dictionary<string, object>();

        internal override void SubscriptionSucceeded(string data)
        {
            Members = ParseMembersList(data);
            base.SubscriptionSucceeded(data);
        }

        internal void AddMember(string data)
        {
            var member = ParseMember(data);

            if (!Members.ContainsKey(member.Key))
                Members.Add(member.Key, member.Value);
            else
                Members[member.Key] = member.Value;

            if (MemberAdded != null)
                MemberAdded(this, member);
        }

        internal void RemoveMember(string data)
        {
            var member = ParseMember(data);

            if (Members.ContainsKey(member.Key))
            {
                Members.Remove(member.Key);

                if (MemberRemoved != null)
                    MemberRemoved(this);
            }
        }

        private Dictionary<string, object> ParseMembersList(string data)
        {
            Dictionary<string, object> members = new Dictionary<string, object>();

            //var dataAsObj = MiniJSON.Json.Deserialize(data);

            // for (int i = 0; i < (int)dataAsObj.presence.count; i++)
            // {
            //     var id = (string)dataAsObj.presence.ids[i];
            //     var val = (object)dataAsObj.presence.hash[id];
            //     members.Add(id, val);
            // }

            return members;
        }

        private KeyValuePair<string, object> ParseMember(string data)
        {
            //var dataAsObj = MiniJSON.Json.Deserialize(data);

            // var id = (string)dataAsObj.user_id;
            // var val = (object)dataAsObj.user_info;

            return new KeyValuePair<string, object>("id", "val");
        }

    }
}
