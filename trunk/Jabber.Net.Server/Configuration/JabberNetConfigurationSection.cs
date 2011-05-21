using System.Configuration;

namespace Jabber.Net.Server.Configuration
{
    class JabberNetConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty(JabberNetConfigurationScheme.LISTENERS, IsRequired = true)]
        [ConfigurationCollection(typeof(JabberNetListenerElement), AddItemName = JabberNetConfigurationScheme.LISTENER)]
        public JabberNetConfigurationCollection<JabberNetListenerElement> Listeners
        {
            get { return (JabberNetConfigurationCollection<JabberNetListenerElement>)this[JabberNetConfigurationScheme.LISTENERS]; }
        }
    }
}
