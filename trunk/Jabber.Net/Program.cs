using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jabber.Net.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var server = new JabberNetServer();
                server.Configure();
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
