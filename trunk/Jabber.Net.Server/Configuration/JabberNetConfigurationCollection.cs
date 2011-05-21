using System.Configuration;

namespace Jabber.Net.Server.Configuration
{
    class JabberNetConfigurationCollection<TElement> : ConfigurationElementCollection where TElement : JabberNetConfigurationElement, new()
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new TElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((JabberNetConfigurationElement)element).Key;
        }
    }
}
