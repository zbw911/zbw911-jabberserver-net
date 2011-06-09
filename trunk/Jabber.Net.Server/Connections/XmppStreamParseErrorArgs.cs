using System;

namespace Jabber.Net.Server.Connections
{
    public class XmppStreamParseErrorArgs : EventArgs
    {
        public Exception Error
        {
            get;
            private set;
        }


        public XmppStreamParseErrorArgs(Exception error)
        {
            Error = error;
        }
    }
}
