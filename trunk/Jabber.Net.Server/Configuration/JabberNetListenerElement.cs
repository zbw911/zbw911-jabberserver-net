using System;
using System.Configuration;

namespace Jabber.Net.Server.Configuration
{
    class JabberNetListenerElement : JabberNetConfigurationElement
    {
        public override object Key
        {
            get { return ListenUri; }
        }

        [ConfigurationProperty(JabberNetConfigurationScheme.LISTEN_URI, IsRequired = true, IsKey = true)]
        public Uri ListenUri
        {
            get { return (Uri)this[JabberNetConfigurationScheme.LISTEN_URI]; }
        }

        [ConfigurationProperty(JabberNetConfigurationScheme.LISTENER_TYPE, IsRequired = true)]
        public string ListenerType
        {
            get { return (string)this[JabberNetConfigurationScheme.LISTENER_TYPE]; }
        }

        [ConfigurationProperty(JabberNetConfigurationScheme.MAX_RECEIVED_MESSAGE_SIZE, DefaultValue = (int)UInt16.MaxValue)]
        public int MaxReceivedMessageSize
        {
            get { return (int)this[JabberNetConfigurationScheme.MAX_RECEIVED_MESSAGE_SIZE]; }
        }
    }
}
