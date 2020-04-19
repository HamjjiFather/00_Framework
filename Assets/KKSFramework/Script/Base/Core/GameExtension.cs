using System.IO;
using KKSFramework.LocalData;
using UnityEngine;

/// <summary>
/// 게임 확장 클래스.
/// </summary>
public static class GameExtension
{
    /// <summary>
    /// Bundle 클래스 Json파일로 저장.
    /// </summary>
    public static Bundle FromJsonData(this Bundle bundle)
    {
        var filePath = $"{Application.persistentDataPath}/{bundle.GetType().Name}.json";
        if (!File.Exists (filePath)) return bundle;
        var dataString = File.ReadAllText(filePath);
        JsonUtility.FromJsonOverwrite(dataString, bundle);

        return bundle;
    }

    /// <summary>
    /// Json파일 Bundle클래스로 저장.
    /// </summary>
    public static void ToJsonData<T>(this Bundle bundle) where T : Bundle
    {
        var filePath = $"{Application.persistentDataPath}/{typeof(T).Name}.json";
        File.WriteAllText(filePath, JsonUtility.ToJson(bundle));
    }
}