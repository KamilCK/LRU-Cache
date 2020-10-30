namespace LeastRecentCache
{
    public interface IDataProvider<M, T>
    {
        T GetData(M request);
    }
}