using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Jabber.Net.Server.Configuration
{
    class JabberNetConfigurationCollection<TElement> : ConfigurationElementCollection, IEnumerable<TElement> where TElement : JabberNetConfigurationElement, new()
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new TElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TElement)element).Key;
        }

        public new IEnumerator<TElement> GetEnumerator()
        {
            var enumerator = base.GetEnumerator();
            while (enumerator.MoveNext())
            {
                yield return (TElement)enumerator.Current;
            }
        }
    }
}
