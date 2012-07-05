using System;
using System.Security.Cryptography.X509Certificates;
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

        bool SupportTls { get; }


        void Send(Element e, Action<Element> error);

        void Close();

        void Reset();

        void StartTls(X509Certificate certificate);
    }
}
