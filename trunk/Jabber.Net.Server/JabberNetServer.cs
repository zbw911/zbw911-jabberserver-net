using System.Configuration;
using Jabber.Net.Server.Connections;
using Jabber.Net.Server.Handlers;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using agsXMPP.protocol;

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

            var c = new Jabber.Net.Server.S2C.ClientStreamHandler();
            HandlerManager.RegisterStreamHandler<Stream>(c.ProcessStream);
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
