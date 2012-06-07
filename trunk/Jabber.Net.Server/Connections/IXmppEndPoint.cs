using System;
using agsXMPP.Xml.Dom;

namespace Jabber.Net.Server.Connections
{
    public interface IXmppEndPoint
    {
        string SessionId
        {
            get;
            set;
        }


        void Send(Element e, Action<Element> error);

        void Close();
    }
}
