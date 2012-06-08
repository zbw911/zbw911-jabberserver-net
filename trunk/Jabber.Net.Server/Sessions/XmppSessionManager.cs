using System.Collections.Generic;
using System.Threading;

namespace Jabber.Net.Server.Sessions
{
    public class XmppSessionManager
    {
        private readonly Dictionary<string, XmppSession> sessions = new Dictionary<string, XmppSession>(1000);
        private readonly ReaderWriterLockSlim locker = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);


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
            locker.EnterReadLock();
            try
            {
                sessions.TryGetValue(id, out s);
            }
            finally
            {
                locker.ExitReadLock();
            }
            return s;
        }

        public void OpenSession(XmppSession session)
        {
            Args.NotNull(session, "session");

            locker.EnterWriteLock();
            try
            {
                sessions[session.Id] = session;
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }

        public void CloseSession(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                XmppSession s;
                locker.EnterWriteLock();
                try
                {
                    if (sessions.TryGetValue(id, out s))
                    {
                        sessions.Remove(id);
                    }
                }
                finally
                {
                    locker.ExitWriteLock();
                }
                if (s != null)
                {
                    s.EndPoint.Close();
                }
            }
        }
    }
}
