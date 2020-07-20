using KKSFramework.Management;
using UniRx.Async;
using UnityEngine;

namespace KKSFramework.ResourcesLoad
{
    public class ResourcesLoadManager : ManagerBase<ResourcesLoadManager>
    {
        #region Methods

        /// <summary>
        /// 리소스 로드.
        /// </summary>
        public T GetResources<T>(string path) where T : Object
        {
            var resourceObject = Resources.Load<T>(path);
            if (resourceObject != null)
                return Resources.Load<T>(path);
            
            Debug.Log($"{nameof(ResourcesLoadManager)}_nullObject");
            return null;

        }


        /// <summary>
        /// 제네릭 타입에 해당하는 리소스 로드.
        /// </summary>
        public T GetResources<T>(string roleString, string typeString, string prefabName) where T : Object
        {
            return GetResources<T>($"{roleString}/{typeString}/{prefabName}");
        }

        /// <summary>
        /// 제네릭 타입에 해당하는 리소스 로드.
        /// </summary>
        public T GetResources<T>(string roleString, string prefabName) where T : Object
        {
            return GetResources<T>($"{roleString}/{prefabName}");
        }


        /// <summary>
        /// 리소스 비동기 로드.
        /// </summary>
        private async UniTask<T> GetResourceAsync<T>(string path) where T : Object
        {
            var resourceObject = await Resources.LoadAsync<T>(path);
            if (resourceObject != null)
                return resourceObject as T;
            
            Debug.Log($"{nameof(ResourcesLoadManager)}_nullObject");
            return null;

        }

        /// <summary>
        /// 제네릭 타입에 해당하는 리소스 비동기 로드.
        /// </summary>
        public async UniTask<T> GetResourcesAsync<T>(string roleString, string typeString, string prefabName)
            where T : Object
        {
            return await GetResourceAsync<T>($"{roleString}/{typeString}/{prefabName}");
        }

        /// <summary>
        /// 제네릭 타입에 해당하는 리소스 비동기 로드.
        /// </summary>
        public async UniTask<T> GetResourcesAsync<T>(string roleString, string prefabName)
            where T : Object
        {
            return await GetResourceAsync<T>($"{roleString}/{prefabName}");
        }

        #endregion
    }
}