namespace LeastRecentCache
{
    public interface ICache<TM, T>
    {
        void RegisterLogger(ILogger logger);
        void RegisterDataProvider(IDataProvider<TM, T> provider);
        T GetData(TM request);
        void ClearCache();
        void ResizeCache(int size);
        int GetCacheFill();

    }
}