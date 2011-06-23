using System;
using Jabber.Net.Server.Configuration;
using Jabber.Net.Server.Connections;
using Jabber.Net.Server.Handlers;
using Jabber.Net.Server.Services;

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

        public XmppHandlerManager HandlerManager
        {
            get;
            private set;
        }

        public XmppServiceManager ServiceManager
        {
            get;
            private set;
        }


        public JabberNetServer()
        {
            HandlerManager = new XmppHandlerManager();
            ConnectionManager = new XmppConnectionManager(HandlerManager);
            ListenerManager = new XmppListenerManager(ConnectionManager);
            ServiceManager = new XmppServiceManager(HandlerManager);
        }


        public void Configure()
        {
            Configure(null);
        }

        public void Configure(string configfile)
        {
            var jabberSection = JabberNetConfigurationSection.Load(configfile);

            foreach (var e in jabberSection.Listeners)
            {
                var listener = (IXmppListener)Activator.CreateInstance(Type.GetType(e.ListenerType, true));
                listener.ListenUri = e.ListenUri;
                listener.MaxReceivedMessageSize = e.MaxReceivedMessageSize;
                ConfigureConfigurable(listener as IConfigurable, e);

                ListenerManager.AddListener(listener);
            }
        }


        public void Start()
        {
            ListenerManager.StartListen();
        }

        public void Stop()
        {
            ListenerManager.StopListen();
        }


        private void ConfigureConfigurable(IConfigurable configurable, JabberNetConfigurationElement element)
        {
            if (configurable != null)
            {
                configurable.Configure(element.UnrecognizedAttributes);
            }
        }
    }
}
