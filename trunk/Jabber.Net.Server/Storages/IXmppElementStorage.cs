using System.Collections.Generic;
using agsXMPP;
using agsXMPP.Xml.Dom;

namespace Jabber.Net.Server.Storages
{
    public interface IXmppElementStorage
    {
        Element GetSingleElement(Jid jid, string key);

        void SaveSingleElement(Jid jid, string key, Element element);

        bool RemoveSingleElement(Jid jid, string key);


        IEnumerable<Element> GetElements(Jid jid, string key);

        void SaveElements(Jid jid, string key, params Element[] element);

        bool RemoveElements(Jid jid, string key);
    }
}
