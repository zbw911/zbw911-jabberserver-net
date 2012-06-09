using System.Collections.Generic;
using Jabber.Net.Server.Utils;

namespace Jabber.Net.Server.Sessions
{
    public class XmppSessionManager
    {
        private readonly Dictionary<string, XmppSession> sessions = new Dictionary<string, XmppSession>(1000);
        private readonly ReaderWriterLock locker = new ReaderWriterLock();


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
            using (locker.ReadLock())
            {
                sessions.TryGetValue(id, out s);
            }
            return s;
        }

        public void OpenSession(XmppSession session)
        {
            Args.NotNull(session, "session");

            using (locker.WriteLock())
            {
                sessions[session.Id] = session;
            }
        }

        public void CloseSession(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                XmppSession s;
                using(locker.WriteLock())
                {
                    if (sessions.TryGetValue(id, out s))
                    {
                        sessions.Remove(id);
                    }
                }
                if (s != null)
                {
                    s.EndPoint.Close();
                }
            }
        }
    }
}
