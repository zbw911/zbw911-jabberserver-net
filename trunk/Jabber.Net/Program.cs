using System;

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
