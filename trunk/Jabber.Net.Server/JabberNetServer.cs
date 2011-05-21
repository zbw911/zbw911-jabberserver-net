using System;
using System.Configuration;
using Jabber.Net.Server.Configuration;
using Jabber.Net.Server.Listeners;
using Jabber.Net.Server.Streams;

namespace Jabber.Net.Server
{
    public class JabberNetServer
    {
        public XmppListenersManager ListenersManager
        {
            get;
            private set;
        }

        public XmppStreamsManager StreamsManager
        {
            get;
            private set;
        }


        public JabberNetServer()
        {
            StreamsManager = new XmppStreamsManager();
            ListenersManager = new XmppListenersManager(StreamsManager);
        }


        public void Configure()
        {
            Configure(null);
        }

        public void Configure(string configfile)
        {
            JabberNetConfigurationSection jabberSection = null;

            if (string.IsNullOrEmpty(configfile))
            {
                jabberSection = (JabberNetConfigurationSection)ConfigurationManager.GetSection(JabberNetConfigurationScheme.SECTION_NAME);
            }
            else
            {
                var configuration = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap { LocalUserConfigFilename = configfile }, ConfigurationUserLevel.None);
                jabberSection = (JabberNetConfigurationSection)configuration.GetSection(JabberNetConfigurationScheme.SECTION_NAME);
            }
            if (jabberSection == null)
            {
                throw new ConfigurationErrorsException("Configuration section 'jabberNet' not found.");
            }

            foreach (JabberNetListenerElement e in jabberSection.Listeners)
            {
                var listener = (IXmppListener)Activator.CreateInstance(e.ListenerType);
                listener.ListenUri = e.ListenUri;
                listener.MaxReceivedMessageSize = e.MaxReceivedMessageSize;
                listener.Configure(e.UnrecognizedAttributes);

                ListenersManager.AddListener(listener);
            }
        }
    }
}
