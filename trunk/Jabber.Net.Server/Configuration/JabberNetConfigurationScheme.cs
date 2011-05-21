using System.Configuration;

namespace Jabber.Net.Server.Configuration
{
    static class JabberNetConfigurationScheme
    {
        public const string SECTION_NAME = "jabber.net";

        public const string LISTENERS = "listeners";

        public const string LISTENER = "listener";

        public const string LISTEN_URI = "listenUri";
        
        public const string LISTENER_TYPE = "listenerType";

        public const string MAX_RECEIVED_MESSAGE_SIZE = "maxReceivedMessageSize";
    }
}
