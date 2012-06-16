using System.Collections.Generic;
using Jabber.Net.Server.Collections;

namespace Jabber.Net.Server.Sessions
{
    public class XmppSessionManager
    {
        private readonly ReaderWriterLockDictionary<string, XmppSession> sessions = new ReaderWriterLockDictionary<string, XmppSession>(1000);


        public IList<AuthMechanism> SupportedAuthMechanisms
        {
            get;
            private set;
        }


        public XmppSessionManager()
        {
            SupportedAuthMechanisms = new List<AuthMechanism>();
        }


        public XmppSession GetSession(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }

            XmppSession s;
            return sessions.TryGetValue(id, out s) ? s : null;
        }

        public void OpenSession(XmppSession session)
        {
            Args.NotNull(session, "session");

            sessions[session.Id] = session;
        }

        public void CloseSession(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                XmppSession s;
                sessions.Remove(id, out s);
                if (s != null)
                {
                    s.EndPoint.Close();
                }
            }
        }
    }
}
