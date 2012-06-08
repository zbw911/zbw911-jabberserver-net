using System;
using System.Configuration;
using System.Data.Common;

namespace Jabber.Net.Server.Storages
{
    public class XmppStorageBase : IDisposable
    {
        private readonly DbProviderFactory dbFactory;


        public XmppStorageBase(ConnectionStringSettings cs)
        {
            dbFactory = DbProviderFactories.GetFactory(cs.ProviderName);
        }

        public void Dispose()
        {
        }
    }
}
