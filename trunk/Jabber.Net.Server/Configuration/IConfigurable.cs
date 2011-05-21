using System.Collections.Generic;

namespace Jabber.Net.Server.Configuration
{
    public interface IConfigurable
    {
        void Configure(IDictionary<string, string> properties);
    }
}
