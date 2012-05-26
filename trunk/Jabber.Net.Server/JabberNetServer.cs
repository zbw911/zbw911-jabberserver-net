using System.Configuration;
using Jabber.Net.Server.Connections;
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

        public XmppConnectionManager ConnectionManager
        {
            get;
            set;
        }


        public void Configure(string file)
        {
            var map = new ExeConfigurationFileMap { ExeConfigFilename = file };
            var configuration = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);

            var unityContainer = new UnityContainer()
                .LoadConfiguration((UnityConfigurationSection)configuration.GetSection("unity"), "Jabber");

            ListenerManager = unityContainer.Resolve<XmppListenerManager>();
            ConnectionManager = unityContainer.Resolve<XmppConnectionManager>();
        }


        public void Start()
        {
            ListenerManager.StartListen(ConnectionManager);
        }

        public void Stop()
        {
            ListenerManager.StopListen();
        }
    }
}
