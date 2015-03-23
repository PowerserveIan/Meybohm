namespace BaseCode
{
    public interface ICacheProvider
    {
        object this[string key] { get; set; }
        object Get(string key);
        void Store(string key, object obj);
        object Remove(string key);
        void Purge(string prefix = null);
		bool IsEnabled { get; }
		bool IsEmptyCacheItem(string key);
    }
}