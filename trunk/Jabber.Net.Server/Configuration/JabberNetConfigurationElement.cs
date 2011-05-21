using System.Configuration;
using System.Collections.Generic;

namespace Jabber.Net.Server.Configuration
{
    abstract class JabberNetConfigurationElement : ConfigurationElement
    {
        public abstract object Key
        {
            get;
        }

        public IDictionary<string, string> UnrecognizedAttributes
        {
            get;
            private set;
        }


        public JabberNetConfigurationElement()
        {
            UnrecognizedAttributes = new Dictionary<string, string>();
        }


        protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
        {
            UnrecognizedAttributes[name] = value;
            return true;
        }
    }
}
