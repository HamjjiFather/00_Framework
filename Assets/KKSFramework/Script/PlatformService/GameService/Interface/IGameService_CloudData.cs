using System;

namespace KKSFramework.PlatformService
{
    public interface IGameServiceCloudData
    {
        string SaveDataName { get; }
        
        void SavedGameData(byte[] data, Action<bool> callback);
        
        void LoadGameData(Action<bool, byte[]> callback);
        
        void DeleteGameData(Action<bool> callback = null);
    }
}