using System;
using agsXMPP.Xml.Dom;
using agsXMPP.protocol.client;

namespace Jabber.Net.Server.Xmpp
{
    public class XmppElement : ICloneable
    {
        public Element Element
        {
            get;
            private set;
        }

        public XmppElement(Element element)
        {
            Element = element;
        }

        public override string ToString()
        {
            return Element.ToString();
        }

        public object Clone()
        {
            return new XmppElement((Element)Element.Clone());
        }
    }
}
