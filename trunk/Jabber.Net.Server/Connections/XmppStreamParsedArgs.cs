using System;
using System.Xml.Linq;

namespace Jabber.Net.Server.Connections
{
    public class XmppStreamParsedArgs : EventArgs
    {
        public XObject Xmpp
        {
            get;
            private set;
        }


        public XmppStreamParsedArgs(XObject xmpp)
        {
            Xmpp = xmpp;
        }
    }
}
