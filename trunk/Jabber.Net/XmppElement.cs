using agsXMPP.Xml.Dom;

namespace Jabber.Net
{
    public class XmppElement
    {
        public Node Node
        {
            get;
            private set;
        }

        public XmppElement(Node node)
        {
            Node = node;
        }

        public override string ToString()
        {
            return Node.ToString();
        }
    }
}
