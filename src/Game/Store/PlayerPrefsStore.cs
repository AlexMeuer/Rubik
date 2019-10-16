using Core.Store;
using UnityEngine;

namespace Game.Store
{
    public class PlayerPrefsStore : IStore
    {
        public bool HasKey(string key) => PlayerPrefs.HasKey(key);
        public void Delete(string key) => PlayerPrefs.DeleteKey(key);

        public void SetString(string key, string value) => PlayerPrefs.SetString(key, value);

        public string GetString(string key, string fallback = null) => PlayerPrefs.GetString(key, fallback);
        
        public void SetBool(string key, bool value) => PlayerPrefs.SetInt(key, value ? 1 : 0);

        public bool GetBool(string key, bool fallback = false) => PlayerPrefs.GetInt(key, fallback ? 1 : 0) != 0;
    }
}