using System;
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

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="authEndpoint">The End point to contact</param>
        public HttpAuthorizer(string authEndpoint, string userId = null)
        {
            _authEndpoint = new Uri(authEndpoint);
            _userId = userId;
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
                webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                authToken = webClient.UploadString(_authEndpoint, "POST", data);
            }

            return authToken;
        }
    }
}