namespace Core.Store
{
    public interface IStore
    {
        bool HasKey(string key);
        void Delete(string key);
        
        void SetString(string key, string value);
        string GetString(string key, string fallback = null);
        
        void SetBool(string key, bool value);
        bool GetBool(string key, bool fallback = false);
    }
}