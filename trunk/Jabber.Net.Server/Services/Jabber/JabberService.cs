using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jabber.Net.Server.Handlers;

namespace Jabber.Net.Server.Services.Jabber
{
    class JabberService : IXmppService
    {
        public string Jid
        {
            get;
            set;
        }

        public void Register(XmppHandlerManager handlerManager)
        {
            handlerManager.AddHandler(new ClientStreamHandler());
        }

        public void Unregister(XmppHandlerManager handlerManager)
        {
            //handlerManager.RemoveHandler();
        }
    }
}
