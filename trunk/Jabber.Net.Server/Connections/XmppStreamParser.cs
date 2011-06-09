using System;
using System.Collections.Generic;
using System.Text;

namespace Jabber.Net.Server.Connections
{
    public class XmppStreamParser
    {
        private readonly IDictionary<Guid, byte[]> buffers;


        public event EventHandler<XmppStreamParsedArgs> Parsed;

        public event EventHandler<XmppStreamParseErrorArgs> Error;


        public XmppStreamParser()
        {
            buffers = new Dictionary<Guid, byte[]>(1000);
        }


        public void Parse(byte[] buffer)
        {
        }

        public void Reset(Guid connectionId)
        {
        }
    }
}
