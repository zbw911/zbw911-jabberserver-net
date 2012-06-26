using System;
using System.Collections.Generic;
using agsXMPP.Xml.Dom;

namespace Jabber.Net.Server.Storages
{
    public interface IXmppElementStorage
    {
        T GetSingleElement<T>(string key);

        void SaveSingleElement(string key, Element element);

        void RemoveSingleElement<T>(string key);


        Element GetSingleElement(string key, Type elementType);

        void RemoveSingleElement(string key, Type elementType);


        IEnumerable<T> GetElements<T>(string key);

        void SaveElements(string key, params Element[] elements);

        void RemoveElements<T>(string key);


        IEnumerable<Element> GetElements(string key, Type elementType);

        void RemoveElements(string key, Type elementType);
    }
}
