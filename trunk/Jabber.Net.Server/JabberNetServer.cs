using System.Configuration;
using Jabber.Net.Server.Connections;
using Jabber.Net.Server.Handlers;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace Jabber.Net.Server
{
    public class JabberNetServer
    {
        public XmppListenerManager ListenerManager
        {
            get;
            set;
        }

        public XmppHandlerManager HandlerManager
        {
            get;
            set;
        }


        public void Configure(string file)
        {
            var map = new ExeConfigurationFileMap { ExeConfigFilename = file };
            var configuration = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            var unitySection = (UnityConfigurationSection)configuration.GetSection(UnityConfigurationSection.SectionName);
            var unityContainer = new UnityContainer()
                .LoadConfiguration(unitySection, "Jabber");

            ListenerManager = unityContainer.Resolve<XmppListenerManager>();
            HandlerManager = unityContainer.Resolve<XmppHandlerManager>();
        }


        public void Start()
        {
            ListenerManager.StartListen();
        }

        public void Stop()
        {
            ListenerManager.StopListen();
        }
    }
}
