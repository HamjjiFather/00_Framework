using UniRx.Async;

namespace KKSFramework.ResourcesLoad
{
    public enum ResourceRoleType
    {
        _Data,
        _Prefab,
        _Sound,
        _Image,
    }

    public enum ResourcesType
    {
        TSV,
        GlobalTextTSV,
        Json,
        
        Page,
        Popup,
        CommonView,

        BGM = 50,
        Button,
        SFX,
    }

    public static class ResourcesLoadHelper
    {
        public static T GetResources<T>(ResourceRoleType roleType, ResourcesType type, string resourceName)
            where T : UnityEngine.Object
        {
            return ResourcesLoadManager.Instance.GetResources<T>(roleType.ToString(), type.ToString(), resourceName);
        }

        public static T GetResources<T>(ResourceRoleType roleType, string resourceName) where T : UnityEngine.Object
        {
            return ResourcesLoadManager.Instance.GetResources<T>(roleType.ToString(), resourceName);
        }


        public static T GetResources<T>(string resourceName) where T : UnityEngine.Object
        {
            return ResourcesLoadManager.Instance.GetResources<T>(resourceName);
        }
        
        
        public static async UniTask<T> GetResourcesAsync<T>(ResourceRoleType roleType, ResourcesType type,
            string resourceName)
            where T : UnityEngine.Object
        {
            return await ResourcesLoadManager.Instance.GetResourcesAsync<T>(roleType.ToString(), type.ToString(),
                resourceName);
        }

        public static async UniTask<T> GetResourcesAsync<T>(ResourceRoleType roleType, string resourceName)
            where T : UnityEngine.Object
        {
            return await ResourcesLoadManager.Instance.GetResourcesAsync<T>(roleType.ToString(), resourceName);
        }
    }
}