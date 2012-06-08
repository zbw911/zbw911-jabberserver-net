using System.Collections.Generic;
using System.Threading;

namespace Jabber.Net.Server.Storages
{
    public class XmppStorageManager
    {
        private readonly Dictionary<string, object> storages = new Dictionary<string, object>();
        private readonly ReaderWriterLockSlim locker = new ReaderWriterLockSlim();


        public IXmppUserStorage Users
        {
            get { return GetStorage<IXmppUserStorage>("users"); }
        }

        
        public void AddStorage(string name, object storage)
        {
            Args.NotNull(name, "name");
            Args.NotNull(storage, "storage");

            locker.EnterWriteLock();
            try
            {
                storages.Add(name, storage);
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }

        public void RemoveStorage(string name)
        {
            Args.NotNull(name, "name");

            locker.EnterWriteLock();
            try
            {
                storages.Remove(name);
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }

        public T GetStorage<T>(string name)
        {
            Args.NotNull(name, "name");

            object storage;
            locker.EnterReadLock();
            try
            {
                return storages.TryGetValue(name, out storage) ? (T)storage : default(T);
            }
            finally
            {
                locker.ExitReadLock();
            }
        }
    }
}
