using System;

namespace Jabber.Net.Server.Connections
{
    public class XmppConnectionCloseArgs : EventArgs
    {
        public Guid ConnectionId
        {
            get;
            private set;
        }

        public byte[][] NotSended
        {
            get;
            private set;
        }


        public XmppConnectionCloseArgs(Guid connectionId, byte[][] notSended)
        {
            NotSended = notSended;
        }
    }
}
