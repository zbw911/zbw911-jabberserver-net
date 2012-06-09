using System;
using System.Threading;

namespace Jabber.Net.Server.Utils
{
    class ReaderWriterLock
    {
        private readonly ReaderWriterLockSlim locker = new ReaderWriterLockSlim();


        public IDisposable ReadLock()
        {
            return new ReaderLocker(locker);
        }

        public IDisposable WriteLock()
        {
            return new WriterLocker(locker);
        }


        private class ReaderLocker : IDisposable
        {
            private readonly ReaderWriterLockSlim _locker;

            public ReaderLocker(ReaderWriterLockSlim locker)
            {
                _locker = locker;
                _locker.EnterReadLock();
            }

            public void Dispose()
            {
                _locker.ExitReadLock();
            }
        }

        private class WriterLocker : IDisposable
        {
            private readonly ReaderWriterLockSlim _locker;

            public WriterLocker(ReaderWriterLockSlim locker)
            {
                _locker = locker;
                _locker.EnterWriteLock();
            }

            public void Dispose()
            {
                _locker.ExitWriteLock();
            }
        }
    }
}
