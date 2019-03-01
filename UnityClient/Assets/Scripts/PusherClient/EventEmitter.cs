using System;
using System.Collections.Generic;

namespace PusherClient
{
    /// <summary>
    /// Class used to Bind and unbind from events
    /// </summary>
    public class EventEmitter
    {
        private readonly Dictionary<string, List<Action<object>>> _eventListeners = new Dictionary<string, List<Action<object>>>();
        private readonly List<Action<string, object>> _generalListeners = new List<Action<string, object>>();

        /// <summary>
        /// Binds to a given event name
        /// </summary>
        /// <param name="eventName">The Event Name to listen for</param>
        /// <param name="listener">The action to perform when the event occurs</param>
        public void Bind(string eventName, Action<object> listener)
        {
            if (_eventListeners.ContainsKey(eventName))
            {
                _eventListeners[eventName].Add(listener);
            }
            else
            {
                var listeners = new List<Action<object>> { listener };
                _eventListeners.Add(eventName, listeners);
            }
        }

        /// <summary>
        /// Binds to ALL event
        /// </summary>
        /// <param name="listener">The action to perform when the any event occurs</param>
        public void BindAll(Action<string, object> listener)
        {
            _generalListeners.Add(listener);
        }

        /// <summary>
        /// Removes the binding for the given event name
        /// </summary>
        /// <param name="eventName">The name of the event to unbind</param>
        public void Unbind(string eventName)
        {
            _eventListeners.Remove(eventName);
        }

        /// <summary>
        /// Remove the action for the event name
        /// </summary>
        /// <param name="eventName">The name of the event to unbind</param>
        /// <param name="listener">The action to remove</param>
        public void Unbind(string eventName, Action<object> listener)
        {
            if (_eventListeners.ContainsKey(eventName))
            {
                _eventListeners[eventName].Remove(listener);
            }
        }

        /// <summary>
        /// Remove All bindings
        /// </summary>
        public void UnbindAll()
        {
            _eventListeners.Clear();
            _generalListeners.Clear();
        }

        internal void EmitEvent(string eventName, string data)
        {

            var obj = MiniJSON.Json.Deserialize(data); ;

            // Emit to general listeners regardless of event type
            foreach (var action in _generalListeners)
            {
                action(eventName, obj);
            }

            if (_eventListeners.ContainsKey(eventName))
            {
                foreach (var action in _eventListeners[eventName])
                {
                    action(obj);
                }
            }
        }
    }
}
