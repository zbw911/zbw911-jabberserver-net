using System.Collections.Generic;
using Jabber.Net.Server.Utils;

namespace Jabber.Net.Server.Storages
{
    public class XmppStorageManager
    {
        private readonly Dictionary<string, object> storages = new Dictionary<string, object>();
        private readonly ReaderWriterLock locker = new ReaderWriterLock();


        public IXmppUserStorage Users
        {
            get { return GetStorage<IXmppUserStorage>("users"); }
        }


        public void AddStorage(string name, object storage)
        {
            Args.NotNull(name, "name");
            Args.NotNull(storage, "storage");

            using (locker.WriteLock())
            {
                storages.Add(name, storage);
            }
        }

        public void RemoveStorage(string name)
        {
            Args.NotNull(name, "name");

            using (locker.WriteLock())
            {
                storages.Remove(name);
            }
        }

        public T GetStorage<T>(string name)
        {
            Args.NotNull(name, "name");

            object storage;
            using (locker.ReadLock())
            {
                return storages.TryGetValue(name, out storage) ? (T)storage : default(T);
            }
        }
    }
}
