using System;
using System.Collections.Generic;
using System.Net;

namespace PusherClient
{
    /// <summary>
    /// An implementation of the <see cref="IAuthorizer"/> using Http
    /// </summary>
    public class HttpAuthorizer : IAuthorizer
    {
        private readonly Uri _authEndpoint;
        private string _userId;
        private string _userData;
        private IDictionary<string, string> _headers = new Dictionary<string, string>();

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="authEndpoint">The End point to contact</param>
        public HttpAuthorizer(string authEndpoint, string userId = null, string userData = null, IDictionary<string, string> headers = null)
        {
            _authEndpoint = new Uri(authEndpoint);
            _userId = userId;
            _userData = userData;
            if (headers != null)
            {
                _headers = headers;
            }
        }

        /// <summary>
        /// Requests the authorisation of channel name
        /// </summary>
        /// <param name="channelName">The channel name to authorize</param>
        /// <param name="socketId">The socket to use during authorization</param>
        /// <returns>the Authorization token</returns>
        public string Authorize(string channelName, string socketId)
        {
            string authToken = null;

            using (var webClient = new WebClient())
            {
                var data = $"channel_name={channelName}&socket_id={socketId}";
                if (_userId != null)
                {
                    data = data + $"&user_id={_userId}";
                }
                webClient.Headers[HttpRequestHeader.ContentType] = "application/json";

                foreach (KeyValuePair<string, string> header in _headers)
                {
                    webClient.Headers[header.Key] = header.Value;
                }

                authToken = webClient.UploadString(_authEndpoint + "?" + data, "POST", _userData);
            }

            return authToken;
        }
    }
}