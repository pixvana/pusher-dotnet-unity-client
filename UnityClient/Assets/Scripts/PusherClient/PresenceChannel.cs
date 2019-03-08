using System.Collections.Generic;
using System.Linq;

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
            var members = new Dictionary<string, object>();

            var dataAsObj = (Dictionary<string, object>)_serializer.Deserialize(data);
            var dataValue = (Dictionary<string, object>)dataAsObj["presence"];

            var ids = ((List<object>)dataValue["ids"]).Cast<string>().ToList();
            var hash = (Dictionary<string, object>)dataValue["hash"];

            foreach (string id in ids)
            {
                if (hash.ContainsKey(id))
                {
                    var userInfo = ParseUserInfo(hash[id]);
                    members.Add(id, userInfo);
                }
                else
                {
                    members.Add(id, null);
                }
            }

            return members;
        }

        private KeyValuePair<string, object> ParseMember(string data)
        {
            var dataAsObj = (Dictionary<string, object>)_serializer.Deserialize(data);
            string id = (string)dataAsObj["user_id"];

            if (dataAsObj.ContainsKey("user_info"))
            {
                return new KeyValuePair<string, object>(id, ParseUserInfo(dataAsObj["user_info"]));
            }

            return new KeyValuePair<string, object>(id, null);
        }

        private Dictionary<string, object> ParseUserInfo(object nodeObj)
        {
            var parsedObject = new Dictionary<string, object>();
            var node = ((Dictionary<string, object>)nodeObj)["node"];
            var children = ((Dictionary<string, object>)node)["_children"];
            foreach (KeyValuePair<string, object> child in ((Dictionary<string, object>)children))
            {
                parsedObject.Add(child.Key, ((Dictionary<string, object>)child.Value)["_value"]);
            }
            return parsedObject;
        }

    }
}