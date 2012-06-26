using System.Collections.Generic;
using agsXMPP;
using agsXMPP.Xml.Dom;

namespace Jabber.Net.Server.Storages
{
    public interface IXmppElementStorage
    {
        Element GetSingleElement(Jid jid, string tag, string ns);

        void SaveSingleElement(Jid jid, Element element);

        void RemoveSingleElement(Jid jid, string tag, string ns);


        IEnumerable<Element> GetElements(Jid jid, string tag, string ns);

        void SaveElements(Jid jid, params Element[] element);

        void RemoveElements(Jid jid, string tag, string ns);
    }
}
