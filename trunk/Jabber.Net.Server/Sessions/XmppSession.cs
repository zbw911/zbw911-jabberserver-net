using agsXMPP;
using Jabber.Net.Server.Connections;
using Jabber.Net.Server.Utils;

namespace Jabber.Net.Server.Sessions
{
    public class XmppSession
    {
        public static readonly XmppSession Empty = new XmppSession(string.Empty);

        public static readonly XmppSession Current = null;

        private static readonly IUniqueId id = new IncrementalUniqueId();
        private IXmppEndPoint endpoint;


        public string Id
        {
            get;
            private set;
        }

        public Jid Jid
        {
            get;
            set;
        }

        public IXmppEndPoint EndPoint
        {
            get { return endpoint; }
            set
            {
                Args.NotNull(value, "EndPoint");
                endpoint = value;
                endpoint.SessionId = Id;
            }
        }

        public bool Authenticated
        {
            get;
            private set;
        }

        public bool Connected
        {
            get;
            private set;
        }

        public object AuthData
        {
            get;
            set;
        }

        public string Language
        {
            get;
            set;
        }


        private XmppSession(string id)
        {
            Id = id;
        }

        public XmppSession(IXmppEndPoint endpoint)
        {
            Args.NotNull(endpoint, "endpoint");

            Id = endpoint.SessionId ?? id.CreateId();
            EndPoint = endpoint;
        }

        public void Authenticate(string username)
        {
            Jid.User = username;
            Authenticated = true;
            AuthData = null;
        }

        public void BindResource(string resource)
        {
            Jid.Resource = resource;
            Connected = true;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var s = obj as XmppSession;
            return s != null && Equals(Id, s.Id);
        }
    }
}
