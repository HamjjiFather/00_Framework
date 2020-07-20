using UnityEngine;

namespace KKSFramework.ResourcesLoad
{
    public static class ObjectPoolingHelper
    {
        #region Methods

        public static T GetResources<T> (ResourceRoleType roleType, ResourcesType type, string resourceName, Transform parents)
            where T : PrefabComponent
        {
            T obj;
            var poolingPath = $"{roleType}/{type}/{resourceName}";
            if (ObjectPoolingManager.Instance.IsExistPooledObject (poolingPath))
            {
                obj = ObjectPoolingManager.Instance.ReturnLoadResources<T> (poolingPath);
                obj.transform.SetParent (parents);
                obj.transform.SetInstantiateTransform ();
                return obj;
            }
            
            var res = ResourcesLoadManager.Instance.GetResources<T> (poolingPath);
            obj = res.InstantiateObject<T> (parents);
            obj.transform.SetInstantiateTransform ();
            return obj;
        }

        #endregion
    }
}