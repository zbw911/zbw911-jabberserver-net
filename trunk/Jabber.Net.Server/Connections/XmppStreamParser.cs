using System;

namespace Jabber.Net.Server.Connections
{
    public class XmppStreamParser
    {
        public event EventHandler<XmppStreamParsedArgs> Parsed;

        public event EventHandler<XmppStreamParseErrorArgs> Error;


        public void Parse(Guid connectionId, byte[] buffer)
        {

        }

        public void Reset(Guid connectionId)
        {

        }
    }
}
