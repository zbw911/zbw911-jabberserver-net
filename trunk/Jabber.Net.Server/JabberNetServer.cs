using System;
using System.Configuration;
using Jabber.Net.Server.Configuration;
using Jabber.Net.Server.Connections;
using Jabber.Net.Server.Listeners;

namespace Jabber.Net.Server
{
    public class JabberNetServer
    {
        public XmppListenerManager ListenerManager
        {
            get;
            private set;
        }

        public XmppConnectionManager ConnectionManager
        {
            get;
            private set;
        }


        public JabberNetServer()
        {
            ConnectionManager = new XmppConnectionManager();
            ListenerManager = new XmppListenerManager(ConnectionManager);
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

                ListenerManager.AddListener(listener);
            }
        }
    }
}
