using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jabber.Net.Server.Connections
{
    public interface IXmppReciever
    {
        void OnRecive(byte[] buffer);

        void OnClose(IEnumerable<byte[]> notSended);
    }
}
