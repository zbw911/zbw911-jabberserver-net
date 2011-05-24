using System;
using System.Xml.Linq;

namespace Jabber.Net.Server.Connections
{
    public class XmppStreamParsedArgs : EventArgs
    {
        public Guid ConnectionId
        {
            get;
            private set;
        }

        public XObject Xmpp
        {
            get;
            private set;
        }


        public XmppStreamParsedArgs(Guid connectionId, XObject xmpp)
        {
            ConnectionId = connectionId;
            Xmpp = xmpp;
        }
    }
}
