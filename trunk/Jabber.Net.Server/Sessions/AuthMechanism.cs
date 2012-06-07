using agsXMPP.protocol.sasl;

namespace Jabber.Net.Server.Sessions
{
    public class AuthMechanism
    {
        public MechanismType Name
        {
            get;
            private set;
        }

        public bool Required
        {
            get;
            private set;
        }

        public AuthMechanism(MechanismType name, bool required)
        {
            Name = name;
            Required = required;
        }
    }
}
