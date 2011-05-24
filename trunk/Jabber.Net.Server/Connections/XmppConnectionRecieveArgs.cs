using System;

namespace Jabber.Net.Server.Connections
{
    public class XmppConnectionRecieveArgs : EventArgs
    {
        public Guid ConnectionId
        {
            get;
            private set;
        }

        public byte[] Buffer
        {
            get;
            private set;
        }


        public XmppConnectionRecieveArgs(Guid connectionId, byte[] buffer)
        {
            ConnectionId = connectionId;
            Buffer = buffer;
        }
    }
}
