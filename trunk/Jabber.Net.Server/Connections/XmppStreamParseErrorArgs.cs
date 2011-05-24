using System;

namespace Jabber.Net.Server.Connections
{
    public class XmppStreamParseErrorArgs : EventArgs
    {
        public Guid ConnectionId
        {
            get;
            private set;
        }

        public Exception Error
        {
            get;
            private set;
        }


        public XmppStreamParseErrorArgs(Guid connectionId, Exception error)
        {
            ConnectionId = connectionId;
            Error = error;
        }
    }
}
