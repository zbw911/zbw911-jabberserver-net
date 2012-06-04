using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Jabber.Net.Server.Connections;
using System.Xml.Linq;
using System.Diagnostics;

namespace Jabber.Net.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var server = new JabberNetServer();
                server.Configure(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                server.Start();

                Console.ReadKey();

                server.Stop();
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
            }
        }
    }
}
