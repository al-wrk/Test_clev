using System.Threading;

namespace Test_clev
{
    public static class Server
    {
        private static int count = 0;
        private static readonly ReaderWriterLockSlim locker = new ReaderWriterLockSlim();

        public static int GetCount()
        {
            try
            {
                locker.EnterReadLock();
                return count;
            }
            finally
            {
                locker.ExitReadLock();
            }
        }

        public static void AddToCount(int value)
        {
            try
            {
                locker.EnterWriteLock();
                count += value;
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }
    }
}