﻿using System;
using agsXMPP.Xml.Dom;
using Jabber.Net.Server.Sessions;
using Jabber.Net.Server.Storages;

namespace Jabber.Net.Server.Handlers.Results
{
    public class XmppSendResult : XmppHandlerResult
    {
        private readonly bool offline;

        private readonly Element element;


        public XmppSendResult(XmppSession session, Element element, bool offline)
            : base(session)
        {
            Args.NotNull(element, "element");

            this.element = element;
            this.offline = offline;
        }

        public override void Execute(XmppHandlerContext context)
        {
            Session.Connection.Send(element, offline ? notsended => Save(notsended, context) : (Action<Element>)null);
        }

        private void Save(Element e, XmppHandlerContext context)
        {
            context.Storages.Elements.SaveOffline(Session.Jid, e);
        }
    }
}
