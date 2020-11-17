namespace LeastRecentCache
{
    public interface IDataProvider<M, T>
    {
        T GetData(M request); //Retrieves data with given key
    }
}