using System;
using System.Linq;
using System.Text;
using Core.Extensions;
using Core.Store;

namespace Game.Cube.Factory
{
    public interface IRubiksCubeSerializer
    {
        bool CanLoadFromStore { get; }
        void SerializeToStore(IRubiksCube cube);
        IRubiksCube LoadFromStore();
        void ClearStoredData();
    }
    
    internal class RubiksCubeSerializer : IRubiksCubeSerializer
    {
        private const string StoreKey = "RubiksCubeData";
        private const char Separator = '¶';
        
        private readonly IStore store;
        private readonly IRubiksCubeFactory rubiksCubeFactory;

        public bool CanLoadFromStore => store.HasKey(StoreKey);

        public RubiksCubeSerializer(IStore store, IRubiksCubeFactory rubiksCubeFactory)
        {
            this.store = store;
            this.rubiksCubeFactory = rubiksCubeFactory;
        }
        
        public void SerializeToStore(IRubiksCube cube)
        {
            var sb = new StringBuilder();

            foreach (var data in cube.GetStickerData())
            {
                sb.Append(data.ToString());

                sb.Append(Separator);
            }
            
            store.SetString(StoreKey, sb.ToString().Base64Encode());
        }

        public IRubiksCube LoadFromStore()
        {
            if (!CanLoadFromStore)
                throw new Exception("No data to load from store!");

            var data = store.GetString(StoreKey).Base64Decode();

            var stickerData = data.Split(Separator)
                                  .Select(StickerData.Deserialize)
                                  .ToList();

            return rubiksCubeFactory.Create(stickerData);
        }

        public void ClearStoredData() => store.Delete(StoreKey);
    }
}