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


        public static JabberNetConfigurationSection Load(string configfile)
        {
            JabberNetConfigurationSection jabberSection = null;
            if (string.IsNullOrEmpty(configfile))
            {
                jabberSection = (JabberNetConfigurationSection)ConfigurationManager.GetSection(JabberNetConfigurationScheme.SECTION_NAME);
            }
            else
            {
                var configuration = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap { LocalUserConfigFilename = configfile }, ConfigurationUserLevel.None);
                jabberSection = (JabberNetConfigurationSection)configuration.GetSection(JabberNetConfigurationScheme.SECTION_NAME);
            }
            if (jabberSection == null)
            {
                throw new ConfigurationErrorsException(string.Format("Configuration section '{0}' not found.", JabberNetConfigurationScheme.SECTION_NAME));
            }

            return jabberSection;
        }
    }
}
