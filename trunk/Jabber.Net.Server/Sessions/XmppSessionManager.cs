using System.Collections.Generic;
using System.Threading;

namespace Jabber.Net.Server.Sessions
{
    public class XmppSessionManager
    {
        private readonly Dictionary<string, XmppSession> sessions = new Dictionary<string, XmppSession>(1000);
        private readonly ReaderWriterLockSlim locker = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);


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
    }
}
